using MarketplaceApp.Data;
using MarketplaceApp.Enums;
using MarketplaceApp.Models;
using MarketplaceApp.ViewModels;
using MarketplaceApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketplaceApp.Controllers
{
    [Authorize]
    public class SwapRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public SwapRequestsController(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // GET: SwapRequests/MyRequests — Inbound swap requests for the user
        public async Task<IActionResult> MyRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var requests = await _context.SwapRequests
                .Include(s => s.OfferedItem).ThenInclude(i => i.Images)
                .Include(s => s.RequestedItem).ThenInclude(i => i.Images)
                .Include(s => s.Requester)
                .Where(s => s.RequestedItem.UserID == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return View(requests);
        }

        // GET: SwapRequests/MySentRequests — Requests sent by the user
        public async Task<IActionResult> MySentRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var requests = await _context.SwapRequests
                .Include(s => s.OfferedItem).ThenInclude(i => i.Images)
                .Include(s => s.RequestedItem).ThenInclude(i => i.Images)
                .Include(s => s.Requester)
                .Where(s => s.RequesterId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return View(requests);
        }

        // =====================================================================
        //  POST: SwapRequests/Respond — Accept or Reject a Swap Request
        //  Uses EF Transaction to ensure safe ownership exchange + Reputation updates
        // =====================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(int id, OfferStatus status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var swapRequest = await _context.SwapRequests
                .Include(s => s.OfferedItem)
                .Include(s => s.RequestedItem)
                .Include(s => s.Requester)
                .FirstOrDefaultAsync(s => s.SwapRequestId == id);

            if (swapRequest == null)
                return Json(new { success = false, message = "Request not found." });

            // Verify the respondent is the owner of the requested item
            if (swapRequest.RequestedItem.UserID != userId)
                return Json(new { success = false, message = "Unauthorized." });

            if (swapRequest.Status != OfferStatus.Pending)
                return Json(new { success = false, message = "Request already processed." });

            if (status == OfferStatus.Rejected)
            {
                swapRequest.Status = OfferStatus.Rejected;
                await _context.SaveChangesAsync();

                // Notify sender using the service
                await _notificationService.NotifySwapRequestRejectedAsync(swapRequest.RequesterId, swapRequest.SwapRequestId, swapRequest.RequestedItem.Title);

                TempData["Message"] = "Swap request rejected.";
                return RedirectToAction(nameof(MyRequests));
            }

            // ===========================================================
            // Accept Request — Real-time ownership exchange via Transaction
            // ===========================================================
            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                var offeredItemOwnerId = swapRequest.OfferedItem.UserID;    // Original Requester
                var requestedItemOwnerId = swapRequest.RequestedItem.UserID; // Original Owner (Respondent)

                // 1. Fetch Users for Reputation Update
                var sender = await _context.Users.FindAsync(offeredItemOwnerId);
                var respondent = await _context.Users.FindAsync(requestedItemOwnerId);

                if (sender == null || respondent == null)
                {
                    await dbTx.RollbackAsync();
                    TempData["Error"] = "User data not found.";
                    return RedirectToAction(nameof(MyRequests));
                }

                // 2. Increment Ratings for both parties
                sender.Rating += 1;
                respondent.Rating += 1;

                // 3. Swap Ownership ✅
                swapRequest.OfferedItem.UserID = requestedItemOwnerId;   // Offered item now belongs to respondent
                swapRequest.RequestedItem.UserID = offeredItemOwnerId;    // Requested item now belongs to requester

                // 4. Update Status to Swapped
                swapRequest.OfferedItem.Status = ItemStatus.Swapped;
                swapRequest.RequestedItem.Status = ItemStatus.Swapped;

                // 5. Update Request Status
                swapRequest.Status = OfferStatus.Accepted;

                // 6. Financial Records (Transaction logs for history)
                _context.Transactions.Add(new Transaction
                {
                    BuyerID = offeredItemOwnerId,
                    SellerID = requestedItemOwnerId,
                    ItemID = swapRequest.OfferedItemId,
                    FinalPrice = swapRequest.OfferedItem.Price,
                    Status = OrderStatus.Completed,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type = TransactionType.SwapRecord,
                    Notes = $"Swap: Gave \"{swapRequest.OfferedItem.Title}\" for \"{swapRequest.RequestedItem.Title}\"",
                    CreatedAt = DateTime.Now
                });

                _context.Transactions.Add(new Transaction
                {
                    BuyerID = requestedItemOwnerId,
                    SellerID = offeredItemOwnerId,
                    ItemID = swapRequest.RequestedItemId,
                    FinalPrice = swapRequest.RequestedItem.Price,
                    Status = OrderStatus.Completed,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type = TransactionType.SwapRecord,
                    Notes = $"Swap: Gave \"{swapRequest.RequestedItem.Title}\" for \"{swapRequest.OfferedItem.Title}\"",
                    CreatedAt = DateTime.Now
                });

                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();

                // 7. Notify both parties
                // To the Requester
                await _notificationService.NotifySwapRequestAcceptedAsync(swapRequest.RequesterId, swapRequest.SwapRequestId, swapRequest.RequestedItem.Title);

                // To the Respondent (Current User)
                await _notificationService.SendAsync(
                    userId!,
                    NotificationType.Order,
                    swapRequest.SwapRequestId,
                    $"🔄 Swap Complete! \"{swapRequest.OfferedItem.Title}\" is now yours.",
                    Url.Action("MyRequests", "SwapRequests")
                );

                TempData["Message"] = "Swap completed successfully! 🎉";
                return RedirectToAction(nameof(MyRequests));
            }
            catch (Exception)
            {
                await dbTx.RollbackAsync();
                TempData["Error"] = "Error processing swap. Please try again.";
                return RedirectToAction(nameof(MyRequests));
            }
        }

    }
}