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
            // 1. ????? ?? ??? ItemIDs ???? ?????? ?? ???? ??? Transactions
            var itemsWithTransactions = await _context.Transactions
                .Select(t => t.ItemID)
                .ToListAsync();

            // 2. ????? ??? ???????? ???? ??? ID ?????? ?? ????? ?? ??????? ???? ???
            var items = await _context.Items
                .Include(i => i.User)
                .Include(i => i.Images)
                .Where(i => !itemsWithTransactions.Contains(i.ItemID)) // ????? ?? ?? ???? ???? ???????
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
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleBlockUser(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsBlocked = !user.IsBlocked;
                await _context.SaveChangesAsync();

                TempData["Success"] = user.IsBlocked ? "User has been blocked." : "User has been unblocked.";
            }
            return RedirectToAction(nameof(ManageUsers));
        }
    }
}