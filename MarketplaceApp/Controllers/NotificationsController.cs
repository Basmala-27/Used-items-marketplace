using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Data;
using System.Security.Claims;

namespace MarketplaceApp.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notifications = await _context.Notifications
                .Where(n => n.UserID == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            var unreadNotifications = notifications.Where(n => !n.IsRead).ToList();
            if (unreadNotifications.Any())
            {
                unreadNotifications.ForEach(n => n.IsRead = true);
                await _context.SaveChangesAsync();
            }

            return View(notifications);
        }

        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Json(0);

            var count = await _context.Notifications
                .CountAsync(n => n.UserID == userId && !n.IsRead);

            return Json(count);
        }
    }
}