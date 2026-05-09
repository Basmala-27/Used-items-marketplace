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

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reviews.Include(r => r.Transaction);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Transaction)
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            ViewData["TransactionID"] = new SelectList(_context.Transactions, "TransactionID", "TransactionID");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewID,TransactionID,ReviewerID,SellerID,Rating,Comment,CreatedAt")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransactionID"] = new SelectList(_context.Transactions, "TransactionID", "TransactionID", review.TransactionID);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["TransactionID"] = new SelectList(_context.Transactions, "TransactionID", "TransactionID", review.TransactionID);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewID,TransactionID,ReviewerID,SellerID,Rating,Comment,CreatedAt")] Review review)
        {
            if (id != review.ReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransactionID"] = new SelectList(_context.Transactions, "TransactionID", "TransactionID", review.TransactionID);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Transaction)
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewID == id);
        }

        // POST: Reviews/SubmitReview
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

            // Validate that the transaction exists, belongs to the current user, is completed, and is with the seller
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.TransactionID == transactionId && t.BuyerID == currentUserId && t.SellerID == sellerId && t.Status == MarketplaceApp.Enums.OrderStatus.Completed);

            if (transaction == null)
            {
                if (isAjax) return Json(new { success = false, message = "Invalid transaction or you are not eligible to review." });
                TempData["ErrorMessage"] = "Invalid transaction or you are not eligible to review.";
                return RedirectToAction("SellerProfile", "Profile", new { id = sellerId });
            }

            // Validate that the user hasn't already reviewed the seller
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
