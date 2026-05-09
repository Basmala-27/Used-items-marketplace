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

        // =====================================================================
        //  GET: Transactions/Index
        // =====================================================================
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transactions
                .Include(t => t.Buyer)
                .Include(t => t.Item)
                .Include(t => t.Seller)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            return View(transactions);
        }

        // =====================================================================
        //  GET: Transactions/MyOrders — Orders as a Buyer
        // =====================================================================
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

            return View(buyRequests);
        }

        // =====================================================================
        //  GET: Transactions/SellerRequests — Inbound Requests for Seller
        // =====================================================================
        public async Task<IActionResult> SellerRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Buy Requests for Seller
            var buyRequests = await _context.BuyRequests
                .Include(b => b.Item).ThenInclude(i => i.Images)
                .Include(b => b.Buyer)
                .Where(b => b.SellerID == userId &&
                            (b.Status == BuyRequestStatus.Pending || b.Status == BuyRequestStatus.SellerAccepted))
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            // Swap Requests for Seller
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

        // =====================================================================
        //  POST: Transactions/BuyItem
        //  Step 1: Buyer reserves item -> 100% funds moved to Pending (Escrow)
        // =====================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyItem(int itemId)
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _context.Items.FindAsync(itemId);

            if (item == null) return Json(new { success = false, message = "Item not found." });
            if (item.UserID == buyerId) return Json(new { success = false, message = "You cannot buy your own item." });
            if (item.Status != ItemStatus.Available) return Json(new { success = false, message = "Item is not available." });

            var buyer = await _context.Users.FindAsync(buyerId);
            if (buyer == null) return Json(new { success = false, message = "User not found." });

            if (buyer.WalletBalance < item.Price)
                return Json(new { success = false, message = "Insufficient balance." });

            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Move funds to Escrow
                buyer.WalletBalance -= item.Price;
                buyer.PendingBalance += item.Price;

                // 2. Reserve the item
                item.Status = ItemStatus.Reserved;

                // 3. Log Initial Transaction
                var escrowTx = new Transaction
                {
                    BuyerID = buyerId!,
                    SellerID = item.UserID,
                    ItemID = item.ItemID,
                    FinalPrice = item.Price,
                    Status = OrderStatus.Pending,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type = TransactionType.Purchase,
                    Notes = $"Escrow Hold: {item.Title}",
                    CreatedAt = DateTime.Now
                };
                _context.Transactions.Add(escrowTx);
                await _context.SaveChangesAsync();

                // 4. Create Buy Request
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

                // 5. Notify Seller
                await _notificationService.NotifyBuyRequestAsync(item.UserID, buyRequest.BuyRequestId, item.Title, item.Price);

                return Json(new { success = true, message = "Buy request sent! Waiting for seller approval. 🎉" });
            }
            catch
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "An error occurred. Please try again." });
            }
        }

        // =====================================================================
        //  POST: Transactions/SellerAcceptBuy 
        //  Step 2: Seller accepts -> First 50% released to Seller
        // =====================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SellerAcceptBuy(int buyRequestId)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var buyRequest = await _context.BuyRequests
                .Include(b => b.Item)
                .Include(b => b.Buyer)
                .FirstOrDefaultAsync(b => b.BuyRequestId == buyRequestId);

            if (buyRequest == null || buyRequest.SellerID != sellerId)
                return Json(new { success = false, message = "Unauthorized or request not found." });

            if (buyRequest.Status != BuyRequestStatus.Pending)
                return Json(new { success = false, message = "This request cannot be accepted." });

            var buyer = await _context.Users.FindAsync(buyRequest.BuyerID);
            var seller = await _context.Users.FindAsync(sellerId);

            var halfAmount = Math.Round(buyRequest.Amount / 2, 2);

            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Release 50% from Escrow to Seller
                buyer.PendingBalance -= halfAmount;
                seller.WalletBalance += halfAmount;

                // 2. Update Statuses
                buyRequest.Status = BuyRequestStatus.SellerAccepted;
                buyRequest.UpdatedAt = DateTime.Now;
                buyRequest.Item.Status = ItemStatus.PendingDelivery;

                // 3. Log Partial Payment
                _context.Transactions.Add(new Transaction
                {
                    BuyerID = buyRequest.BuyerID,
                    SellerID = sellerId!,
                    ItemID = buyRequest.ItemID,
                    FinalPrice = halfAmount,
                    Status = OrderStatus.PartiallyPaid,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type = TransactionType.PartialPayment,
                    Notes = $"Partial Payment (50%) for: {buyRequest.Item.Title}",
                    CreatedAt = DateTime.Now
                });

                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();

                // 4. Notify Buyer
                await _notificationService.NotifyBuyRequestAcceptedAsync(buyRequest.BuyerID, buyRequest.BuyRequestId, buyRequest.Item.Title);

                return Json(new { success = true, message = $"Request accepted! ${halfAmount:0.00} transferred to your wallet. ✅" });
            }
            catch
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "An error occurred." });
            }
        }

        // =====================================================================
        //  POST: Transactions/ConfirmDelivery
        //  Step 3: Buyer confirms -> Final 50% released + Ownership Transfer + Reputation Boost
        // =====================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelivery(int buyRequestId)
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var buyRequest = await _context.BuyRequests
                .Include(b => b.Item)
                .FirstOrDefaultAsync(b => b.BuyRequestId == buyRequestId);

            if (buyRequest == null || buyRequest.BuyerID != buyerId)
                return Json(new { success = false, message = "Unauthorized." });

            if (buyRequest.Status != BuyRequestStatus.SellerAccepted)
                return Json(new { success = false, message = "Delivery cannot be confirmed at this stage." });

            var buyer = await _context.Users.FindAsync(buyerId);
            var seller = await _context.Users.FindAsync(buyRequest.SellerID);

            var remainingAmount = Math.Round(buyRequest.Amount / 2, 2);

            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Release final 50% to Seller
                buyer.PendingBalance -= remainingAmount;
                seller.WalletBalance += remainingAmount;

                // 2. Reputation update
                seller.Rating += 1;
                if (seller.TrustScore < 5.0)
                {
                    seller.TrustScore = Math.Min(5.0, seller.TrustScore + 0.5);
                }

                // 3. Complete the cycle: Sold status + TRANSFER OWNERSHIP ✅
                buyRequest.Item.Status = ItemStatus.Sold;
                buyRequest.Item.UserID = buyerId!;

                buyRequest.Status = BuyRequestStatus.Completed;
                buyRequest.UpdatedAt = DateTime.Now;

                // 4. Log Final Payment
                _context.Transactions.Add(new Transaction
                {
                    BuyerID = buyerId!,
                    SellerID = buyRequest.SellerID,
                    ItemID = buyRequest.ItemID,
                    FinalPrice = remainingAmount,
                    Status = OrderStatus.Completed,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type = TransactionType.FinalPayment,
                    Notes = $"Final Payment (50%) for: {buyRequest.Item.Title}",
                    CreatedAt = DateTime.Now
                });

                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();

                // 5. Notify Seller
                await _notificationService.NotifyOrderCompletedAsync(buyRequest.SellerID, buyRequest.BuyRequestId, buyRequest.Item.Title, remainingAmount);

                return Json(new { success = true, message = "Delivery confirmed! The item is now yours. 🎉" });
            }
            catch
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "Error during confirmation." });
            }
        }

        // =====================================================================
        //  POST: Transactions/TopUpWallet
        // =====================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopUpWallet(decimal amount)
        {
            if (amount <= 0 || amount > 10000)
                return Json(new { success = false, message = "Amount must be between $1 and $10,000." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Json(new { success = false, message = "User not found." });

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
                    Notes = $"Wallet Deposit: ${amount:0.00}",
                    CreatedAt = DateTime.Now
                });

                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();

                return Json(new { success = true, message = "Wallet topped up successfully!" });
            }
            catch
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "Deposit failed." });
            }
        }
    }
}