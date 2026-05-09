using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Data;
using MarketplaceApp.Enums;
using MarketplaceApp.Models;
using MarketplaceApp.Services;

namespace MarketplaceApp.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public TransactionsController(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // ==========================================
        //  VIEW: My Orders (Buyer Perspective)
        // ==========================================
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var buyRequests = await _context.BuyRequests
                .Include(b => b.Item).ThenInclude(i => i.Images)
                .Include(b => b.Seller)
                .Where(b => b.BuyerID == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            var user = await _context.Users.FindAsync(userId);
            ViewBag.WalletBalance = user?.WalletBalance ?? 0;
            ViewBag.PendingBalance = user?.PendingBalance ?? 0;
            var reviewableBuyRequestIds = await GetReviewableBuyRequestIdsAsync(userId!, buyRequests);
            ViewBag.ReviewableBuyRequestIds = reviewableBuyRequestIds;
            ViewBag.ReviewTransactionIds = await GetReviewTransactionLookupAsync(userId!, buyRequests, reviewableBuyRequestIds);

            var pendingReviewPrompt = await GetPendingReviewPromptAsync(userId!, buyRequests, reviewableBuyRequestIds);
            if (pendingReviewPrompt != null)
            {
                ViewBag.PendingReviewPrompt = pendingReviewPrompt;
            }

            return View(buyRequests);
        }

        // ==========================================
        //  VIEW: Seller Requests (Inbound)
        // ==========================================
        public async Task<IActionResult> SellerRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var buyRequests = await _context.BuyRequests
                .Include(b => b.Item).ThenInclude(i => i.Images)
                .Include(b => b.Buyer)
                .Where(b => b.SellerID == userId &&
                      (b.Status == BuyRequestStatus.Pending || b.Status == BuyRequestStatus.SellerAccepted))
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            var swapRequests = await _context.SwapRequests
                .Include(s => s.OfferedItem).ThenInclude(i => i.Images)
                .Include(s => s.RequestedItem).ThenInclude(i => i.Images)
                .Include(s => s.Requester)
                .Where(s => s.RequestedItem.UserID == userId && s.Status == OfferStatus.Pending)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            ViewBag.BuyRequests = buyRequests;
            ViewBag.SwapRequests = swapRequests;

            return View();
        }

        // ==========================================
        //  ACTION: Buy Item (Initiate Escrow)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyItem(int itemId)
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _context.Items.FindAsync(itemId);

            if (item == null) return Json(new { success = false, message = "Item not found." });
            if (item.UserID == buyerId) return Json(new { success = false, message = "You cannot buy your own item." });
            if (item.Status != ItemStatus.Available) return Json(new { success = false, message = "Item is unavailable." });

            var buyer = await _context.Users.FindAsync(buyerId);
            if (buyer.WalletBalance < item.Price)
                return Json(new { success = false, message = "Insufficient balance." });

            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                buyer.WalletBalance -= item.Price;
                buyer.PendingBalance += item.Price;
                item.Status = ItemStatus.Reserved;

                var escrowTx = new Transaction
                {
                    BuyerID = buyerId!,
                    SellerID = item.UserID,
                    ItemID = item.ItemID,
                    FinalPrice = item.Price,
                    Status = OrderStatus.Pending,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type = TransactionType.Purchase,
                    Notes = $"Escrow Hold for: {item.Title}",
                    CreatedAt = DateTime.Now
                };
                _context.Transactions.Add(escrowTx);
                await _context.SaveChangesAsync();

                var buyRequest = new BuyRequest
                {
                    BuyerID = buyerId!,
                    SellerID = item.UserID,
                    ItemID = item.ItemID,
                    Amount = item.Price,
                    Status = BuyRequestStatus.Pending,
                    EscrowTransactionId = escrowTx.TransactionID,
                    CreatedAt = DateTime.Now
                };
                _context.BuyRequests.Add(buyRequest);

                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();

                await _notificationService.NotifyBuyRequestAsync(item.UserID, buyRequest.BuyRequestId, item.Title, item.Price);

                return Json(new { success = true, message = "Purchase request sent! Funds held in Escrow." });
            }
            catch
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "An error occurred." });
            }
        }

        // ==========================================
        //  ACTION: Seller Accepts (Release 50%)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SellerAcceptBuy(int buyRequestId)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var buyRequest = await _context.BuyRequests.Include(b => b.Item).Include(b => b.Buyer)
                .FirstOrDefaultAsync(b => b.BuyRequestId == buyRequestId);

            if (buyRequest == null || buyRequest.SellerID != sellerId)
                return Json(new { success = false, message = "Unauthorized." });

            var buyer = await _context.Users.FindAsync(buyRequest.BuyerID);
            var seller = await _context.Users.FindAsync(sellerId);
            var halfAmount = Math.Round(buyRequest.Amount / 2, 2);

            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                buyer.PendingBalance -= halfAmount;
                seller.WalletBalance += halfAmount;

                buyRequest.Status = BuyRequestStatus.SellerAccepted;
                buyRequest.Item.Status = ItemStatus.PendingDelivery;

                _context.Transactions.Add(new Transaction
                {
                    BuyerID = buyRequest.BuyerID,
                    SellerID = sellerId!,
                    ItemID = buyRequest.ItemID,
                    FinalPrice = halfAmount,
                    Status = OrderStatus.PartiallyPaid,
                    Type = TransactionType.PartialPayment,
                    CreatedAt = DateTime.Now
                });

                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();

                await _notificationService.NotifyBuyRequestAcceptedAsync(buyRequest.BuyerID, buyRequest.BuyRequestId, buyRequest.Item.Title);
                return Json(new { success = true, message = "Accepted! 50% released to your wallet." });
            }
            catch
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "Error processing request." });
            }
        }

        // ==========================================
        //  ACTION: Confirm Delivery (Release Final 50%)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelivery(int buyRequestId)
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var buyRequest = await _context.BuyRequests.Include(b => b.Item)
                .FirstOrDefaultAsync(b => b.BuyRequestId == buyRequestId);

            if (buyRequest == null || buyRequest.BuyerID != buyerId)
                return Json(new { success = false, message = "Unauthorized." });

            var buyer = await _context.Users.FindAsync(buyerId);
            var seller = await _context.Users.FindAsync(buyRequest.SellerID);
            var remaining = Math.Round(buyRequest.Amount / 2, 2);

            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                buyer.PendingBalance -= remaining;
                seller.WalletBalance += remaining;

                // Reputation Update
                seller.Rating += 1;
                seller.TrustScore = Math.Min(5.0, seller.TrustScore + 0.2);

                // Transfer Ownership
                buyRequest.Item.Status = ItemStatus.Sold;
                buyRequest.Item.UserID = buyerId!;

                buyRequest.Status = BuyRequestStatus.Completed;

                var finalPaymentTransaction = new Transaction
                {
                    BuyerID = buyerId!,
                    SellerID = buyRequest.SellerID,
                    ItemID = buyRequest.ItemID,
                    FinalPrice = remaining,
                    Status = OrderStatus.Completed,
                    Type = TransactionType.FinalPayment,
                    CreatedAt = DateTime.Now
                };
                _context.Transactions.Add(finalPaymentTransaction);

                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();

                await _notificationService.NotifyOrderCompletedAsync(buyRequest.SellerID, buyRequest.BuyRequestId, buyRequest.Item.Title, remaining);
                var canReview = !await _context.Reviews
                    .AnyAsync(r => r.ReviewerID == buyerId && r.SellerID == buyRequest.SellerID);

                return Json(new
                {
                    success = true,
                    message = "Delivery confirmed! Item ownership transferred.",
                    reviewPrompt = new
                    {
                        buyRequestId = buyRequest.BuyRequestId,
                        sellerId = buyRequest.SellerID,
                        sellerName = seller?.Name ?? seller?.UserName ?? "Seller",
                        itemTitle = buyRequest.Item?.Title ?? "Item",
                        transactionId = finalPaymentTransaction.TransactionID,
                        canReview
                    }
                });
            }
            catch
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "Error during confirmation." });
            }
        }

        private async Task<List<int>> GetReviewableBuyRequestIdsAsync(string buyerId, List<BuyRequest> buyRequests)
        {
            var completedSellerIds = buyRequests
                .Where(b => b.Status == BuyRequestStatus.Completed)
                .Select(b => b.SellerID)
                .Distinct()
                .ToList();

            if (!completedSellerIds.Any())
            {
                return new List<int>();
            }

            var reviewedSellerIds = await _context.Reviews
                .Where(r => r.ReviewerID == buyerId && completedSellerIds.Contains(r.SellerID))
                .Select(r => r.SellerID)
                .Distinct()
                .ToListAsync();

            return buyRequests
                .Where(b => b.Status == BuyRequestStatus.Completed && !reviewedSellerIds.Contains(b.SellerID))
                .Select(b => b.BuyRequestId)
                .ToList();
        }

        private async Task<object?> GetPendingReviewPromptAsync(string buyerId, List<BuyRequest> buyRequests, List<int> reviewableBuyRequestIds)
        {
            if (!reviewableBuyRequestIds.Any())
            {
                return null;
            }

            var firstPending = buyRequests
                .FirstOrDefault(b => reviewableBuyRequestIds.Contains(b.BuyRequestId));
            if (firstPending == null)
            {
                return null;
            }

            var transactionId = await _context.Transactions
                .Where(t => t.BuyerID == buyerId
                            && t.SellerID == firstPending.SellerID
                            && t.ItemID == firstPending.ItemID
                            && t.Type == TransactionType.FinalPayment
                            && t.Status == OrderStatus.Completed)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => t.TransactionID)
                .FirstOrDefaultAsync();

            if (transactionId <= 0)
            {
                return null;
            }

            return new
            {
                buyRequestId = firstPending.BuyRequestId,
                sellerId = firstPending.SellerID,
                sellerName = firstPending.Seller?.Name ?? firstPending.Seller?.UserName ?? "Seller",
                itemTitle = firstPending.Item?.Title ?? "Item",
                transactionId,
                canReview = true
            };
        }

        private async Task<Dictionary<int, int>> GetReviewTransactionLookupAsync(string buyerId, List<BuyRequest> buyRequests, List<int> reviewableBuyRequestIds)
        {
            var reviewableRequests = buyRequests
                .Where(b => reviewableBuyRequestIds.Contains(b.BuyRequestId))
                .ToList();

            var transactionIds = await _context.Transactions
                .Where(t => t.BuyerID == buyerId
                            && t.Type == TransactionType.FinalPayment
                            && t.Status == OrderStatus.Completed)
                .Select(t => new { t.TransactionID, t.SellerID, t.ItemID, t.CreatedAt })
                .ToListAsync();

            var lookup = new Dictionary<int, int>();
            foreach (var req in reviewableRequests)
            {
                var matchingTransaction = transactionIds
                    .Where(t => t.SellerID == req.SellerID && t.ItemID == req.ItemID)
                    .OrderByDescending(t => t.CreatedAt)
                    .FirstOrDefault();

                if (matchingTransaction != null)
                {
                    lookup[req.BuyRequestId] = matchingTransaction.TransactionID;
                }
            }

            return lookup;
        }

        // ==========================================
        //  GET: TopUp Page
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> TopUp()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);
            ViewBag.CurrentBalance = user?.WalletBalance ?? 0;
            return View();
        }

        // ==========================================
        //  POST: TopUp Logic
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopUpWallet(decimal amount)
        {
            if (amount <= 0 || amount > 10000)
                return Json(new { success = false, message = "Amount must be between $1 and $10,000." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);

            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                user.WalletBalance += amount;
                _context.Transactions.Add(new Transaction
                {
                    BuyerID = userId!,
                    FinalPrice = amount,
                    Status = OrderStatus.Completed,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type = TransactionType.Deposit,
                    Notes = $"Top-up: ${amount:0.00}",
                    CreatedAt = DateTime.Now
                });
                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();
                return Json(new { success = true, message = "Wallet topped up successfully!" });
            }
            catch
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "Transaction failed." });
            }
        }
    }
}