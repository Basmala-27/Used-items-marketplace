using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Models;
using MarketplaceApp.ViewModels;
using MarketplaceApp.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MarketplaceApp.Enums;
namespace MarketplaceApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(ApplicationDbContext context,
                                 UserManager<ApplicationUser> userManager,
                                 IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var viewModel = new ProfileVM
            {
                Name = user.Name,
                Image = user.ProfileImage,
                MemberSince = user.CreatedAt,
                ItemsPostedCount = await _context.Items.CountAsync(i => i.UserID == user.Id),
                PostedItems = await _context.Items
                    .Where(i => i.UserID == user.Id)
                    .Include(i => i.Images)
                    .ToListAsync(),
                FavoritesCount = await _context.Favorites.CountAsync(f => f.UserID == user.Id),
                FavoriteItems = await _context.Favorites
                    .Where(f => f.UserID == user.Id)
                    .Include(f => f.Item).ThenInclude(i => i.Images)
                    .Select(f => f.Item)
                    .ToListAsync(),

                SwapRequests = await _context.SwapRequests
                    .CountAsync(s => s.RequesterId == user.Id || s.RequestedItem.UserID == user.Id),

                Transactions = await _context.Transactions
                    .Include(t => t.Item)
                    .Where(t => t.BuyerID == user.Id || t.SellerID == user.Id)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new EditProfileVM
            {
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                ExistingImageUrl = user.ProfileImage
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            try
            {
                if (model.ProfileImage != null)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(model.ProfileImage.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ProfileImage", "Only .jpg, .jpeg, and .png files are allowed.");
                        return View(model);
                    }

                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/profiles");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfileImage.CopyToAsync(fileStream);
                    }

                    user.ProfileImage = "/images/profiles/" + uniqueFileName;
                }

                user.Name = model.Name;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred during the update.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Password has been changed successfully!";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(int itemId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserID == user.Id && f.ItemID == itemId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> SellerProfile(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var seller = await _userManager.FindByIdAsync(id);
            if (seller == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isOwner = currentUserId == id;

            var itemsQuery = _context.Items.Where(i => i.UserID == id);

            if (!isOwner)
            {
                itemsQuery = itemsQuery.Where(i => i.Status == ItemStatus.Available);
            }

            var availableItems = await itemsQuery
                .Include(i => i.Images)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            var reviews = await _context.Reviews
                .Include(r => r.Reviewer)
                .Where(r => r.SellerID == id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            double averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
            int totalReviews = reviews.Count;

            int? eligibleTransactionIdToReview = null;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (currentUserId != null && currentUserId != id)
                {
                    var completedTransaction = await _context.Transactions
                        .Where(t => t.BuyerID == currentUserId && t.SellerID == id && t.Status == OrderStatus.Completed)
                        .OrderByDescending(t => t.CreatedAt)
                        .FirstOrDefaultAsync();

                    if (completedTransaction != null)
                    {
                        var hasReviewed = await _context.Reviews
                            .AnyAsync(r => r.ReviewerID == currentUserId && r.SellerID == id);
                        if (!hasReviewed)
                        {
                            eligibleTransactionIdToReview = completedTransaction.TransactionID;
                        }
                    }
                }
            }

            var vm = new SellerProfileVM
            {
                Seller = seller,
                AverageRating = averageRating,
                TotalReviews = totalReviews,
                AvailableItems = availableItems,
                Reviews = reviews,
                EligibleTransactionIdToReview = eligibleTransactionIdToReview
            };

            return View(vm);
        }
    }
}