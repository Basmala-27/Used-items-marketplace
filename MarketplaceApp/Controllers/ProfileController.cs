using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Models;
using MarketplaceApp.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MarketplaceApp.Controllers
{
    [Authorize] // لضمان أن المستخدمين المسجلين فقط هم من يصلون لهذه الأفعال
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

        // 1. عرض البروفايل (المنتجات والمفضلات والعمليات)
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
                ActiveOffers = await _context.Offers.CountAsync(o => o.BuyerID == user.Id),
                SwapRequests = await _context.SwapRequests
                    .CountAsync(s => s.RequesterId == user.Id || s.RequestedItem.UserID == user.Id),

                // --- NEW: Fetch Transactions for the current user ---
                Transactions = await _context.Transactions
                    .Include(t => t.Item) // To show the item name
                    .Where(t => t.BuyerID == user.Id || t.SellerID == user.Id)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // 2. GET: تعديل البيانات الأساسية
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

        // 3. POST: حفظ تعديلات البيانات الأساسية والصورة
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
                    // فحص الامتداد للأمان
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

        // 4. GET: صفحة تغيير الباسورد
        [HttpGet]
        public IActionResult ChangePassword() => View();

        // 5. POST: تغيير الباسورد (مهندل بشروط الـ Identity)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            // ميثود Identity الجاهزة للتحقق من القديم وعمل Hash للجديد
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Password has been changed successfully!";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                // هنا الـ Identity هيطلع Errors لو الباسورد الجديد مش فيه Capital Letters مثلاً
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // 6. حذف من المفضلات
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
    }
}