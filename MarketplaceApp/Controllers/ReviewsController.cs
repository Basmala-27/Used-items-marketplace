using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Data;
using MarketplaceApp.Models;
using System.Security.Claims;

namespace MarketplaceApp.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> SubmitReview(int transactionId, string sellerId, int rating, string comment)
        {
            var currentUserId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (currentUserId == null)
            {
                if (isAjax) return Json(new { success = false, message = "Unauthorized." });
                return Unauthorized();
            }

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.TransactionID == transactionId && t.BuyerID == currentUserId && t.SellerID == sellerId && t.Status == MarketplaceApp.Enums.OrderStatus.Completed);

            if (transaction == null)
            {
                if (isAjax) return Json(new { success = false, message = "Invalid transaction or you are not eligible to review." });
                TempData["ErrorMessage"] = "Invalid transaction or you are not eligible to review.";
                return RedirectToAction("SellerProfile", "Profile", new { id = sellerId });
            }

            var hasReviewed = await _context.Reviews
                .AnyAsync(r => r.ReviewerID == currentUserId && r.SellerID == sellerId);

            if (hasReviewed)
            {
                if (isAjax) return Json(new { success = false, message = "You have already reviewed this seller." });
                TempData["ErrorMessage"] = "You have already reviewed this seller.";
                return RedirectToAction("SellerProfile", "Profile", new { id = sellerId });
            }

            if (rating < 1 || rating > 5)
            {
                if (isAjax) return Json(new { success = false, message = "Rating must be between 1 and 5." });
                TempData["ErrorMessage"] = "Rating must be between 1 and 5.";
                return RedirectToAction("SellerProfile", "Profile", new { id = sellerId });
            }

            var review = new Review
            {
                TransactionID = transactionId,
                ReviewerID = currentUserId,
                SellerID = sellerId,
                Rating = rating,
                Comment = string.IsNullOrEmpty(comment) ? string.Empty : comment,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            if (isAjax) return Json(new { success = true, message = "Review submitted successfully!" });
            TempData["SuccessMessage"] = "Review submitted successfully!";
            return RedirectToAction("SellerProfile", "Profile", new { id = sellerId });
        }
    }
}