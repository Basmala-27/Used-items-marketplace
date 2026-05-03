using MarketplaceApp.Data;
using MarketplaceApp.Enums;
using MarketplaceApp.Models;
using MarketplaceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketplaceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly INotificationService _notificationService;

        public HomeController(
          ApplicationDbContext context,
          ILogger<HomeController> logger,
          INotificationService notificationService)
        {
            _context = context;
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {

            var items = await _context.Items
             .Include(i => i.Images)
             .Include(i => i.User)
             .Include(i => i.Category)
             .OrderByDescending(i => i.CreatedAt)
             .ToListAsync();

            List<int> userFavoriteIds = new List<int>();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {

                    try
                    {
                        await _notificationService.SendAsync(userId, NotificationType.Info, null, "Welcome for comming Back!!");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error sending initial notification");
                    }


                    userFavoriteIds = await _context.Favorites
                      .Where(f => f.UserID == userId)
                      .Select(f => f.ItemID)
                      .ToListAsync();
                }
            }

            ViewBag.UserFavoriteIds = userFavoriteIds;
            return View(items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}