using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Data;
using MarketplaceApp.Models;

namespace MarketplaceApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {

            ViewBag.TotalItems = _context.Items.Count();
            ViewBag.TotalUsers = _context.Users.Count();
            return View();
        }


        public async Task<IActionResult> ManageItems()
        {
            var items = await _context.Items
                .Include(i => i.User)   // جلب بيانات المستخدم
                .Include(i => i.Images) // السطر الناقص: جلب الصور المرتبطة بالمنتج
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return View(items);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                var item = await _context.Items
                    .Include(i => i.Images)
                    .FirstOrDefaultAsync(i => i.ItemID == id);

                if (item != null)
                {
                    // 1. Delete the item (EF Core will cascade delete Images due to DbContext config)

                    // 2. Manual cleanup for SQLite
                    var favorites = _context.Favorites.Where(f => f.ItemID == id);
                    _context.Favorites.RemoveRange(favorites);

                    var conversations = _context.Conversations.Where(c => c.ItemID == id);
                    _context.Conversations.RemoveRange(conversations);

                    var buyRequests = _context.BuyRequests.Where(b => b.ItemID == id);
                    _context.BuyRequests.RemoveRange(buyRequests);

                    var complaints = _context.Complaints.Where(c => c.TargetItemId == id);
                    foreach (var oc in complaints)
                    {
                        oc.TargetItemId = null;
                    }

                    // 3. Delete the item (EF Core will cascade delete Images due to DbContext config)
                    _context.Items.Remove(item);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "The item has been completely removed from the database.";
                }
                else
                {
                    TempData["Error"] = "Item not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting item. " + ex.Message;
            }

            return RedirectToAction(nameof(ManageItems));
        }
        // 4. عرض كل المستخدمين للتحكم في البلوك
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // 5. أكشن عمل بلوك أو فك البلوك عن يوزر
        [HttpPost]
        public async Task<IActionResult> ToggleBlockUser(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                // بنعكس الحالة: لو true تبقى false والعكس
                user.IsBlocked = !user.IsBlocked;
                await _context.SaveChangesAsync();

                TempData["Success"] = user.IsBlocked ? "User has been blocked." : "User has been unblocked.";
            }
            return RedirectToAction(nameof(ManageUsers));
        }
    }
}