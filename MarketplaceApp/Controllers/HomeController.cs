using MarketplaceApp.Data;
using MarketplaceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims; // ??? ???? ???? ??? ID ???? ??????

namespace MarketplaceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // ??? ???????? ?? ?? ???????? ????????
            var items = await _context.Items
                .Include(i => i.Images)
                .Include(i => i.User)
                .Include(i => i.Category)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            // ???? ???? ??? IDs ?????? ?????????
            List<int> userFavoriteIds = new List<int>();

            // ?? ?????? ???? ????? ????? ??????? ?? ??? SQLite
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                userFavoriteIds = await _context.Favorites
                    .Where(f => f.UserID == userId)
                    .Select(f => f.ItemID)
                    .ToListAsync();
            }

            // ????? ?????? ??? View
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