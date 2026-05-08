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
                .Include(i => i.User) // عشان نعرف مين صاحب المنتج
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
                // 1. حذف المفضلات
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Favorites WHERE ItemID = {0}", id);

                // 2. حذف الصور
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM ItemImages WHERE ItemID = {0}", id);

                // 3. حذف العروض (Offers)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Offers WHERE ItemID = {0}", id);

                // 4. حذف طلبات التبديل (كطلب مطلوب أو كعرض مقدم)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM SwapRequests WHERE RequestedItemId = {0} OR OfferedItemId = {0}", id);

                // 5. حذف أي معاملات (Transactions)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Transactions WHERE ItemID = {0}", id);

                // 6. أخيراً حذف المنتج نفسه
                var result = await _context.Database.ExecuteSqlRawAsync("DELETE FROM Items WHERE ItemID = {0}", id);

                if (result > 0)
                {
                    TempData["Success"] = "The item has been completely removed from the database.";
                }
                else
                {
                    TempData["Error"] = "Item not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Fatal Error: " + ex.Message;
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