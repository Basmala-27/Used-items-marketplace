using MarketplaceApp.Data;
using MarketplaceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketplaceApp.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Favorites (??? ????? ???????? ?????? ?????? ???)
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var favorites = await _context.Favorites
                .Where(f => f.UserID == userId)
                .Include(f => f.Item)
                .ThenInclude(i => i.Images)
                .ToListAsync();

            return View(favorites);
        }

        // POST: Favorites/ToggleFavorite (??? Action ???? ??????? ??? AJAX)
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Json(new
                {
                    success = false,
                    isNotLoggedIn = true,
                    redirectUrl = "/Account/Login" 
                });
            }

            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.ItemID == itemId && f.UserID == userId);

            if (existingFavorite != null)
            {
                _context.Favorites.Remove(existingFavorite);
                await _context.SaveChangesAsync();
                return Json(new { success = true, status = "removed" });
            }
            else
            {
                var favorite = new Favorite
                {
                    UserID = userId,
                    ItemID = itemId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
                return Json(new { success = true, status = "added" });
            }
        }

        // GET: Favorites/Delete/5 (????? ?? ???? ????????)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var favorite = await _context.Favorites
                .Include(f => f.Item)
                .FirstOrDefaultAsync(m => m.FavoriteID == id);

            if (favorite == null) return NotFound();

            return View(favorite);
        }

        // POST: Favorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteExists(int id)
        {
            return _context.Favorites.Any(e => e.FavoriteID == id);
        }
    }
}