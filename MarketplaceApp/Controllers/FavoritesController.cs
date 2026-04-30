using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Data;
using MarketplaceApp.Models;
using System.Security.Claims;

namespace MarketplaceApp.Controllers
{
    public class FavoritesController : Controller
    {
        // 1. تعريف الـ context عشان تقدر الميثودز تشوف قاعدة البيانات
        private readonly ApplicationDbContext _context;

        // 2. الـ Constructor لعمل Dependency Injection
        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- الأكشن بتاع القلب (Toggle) ---
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int itemId)
        {
            // بنجيب الـ ID بتاع اليوزر اللي عامل Login حالياً
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // لو مش مسجل دخول، بنبعت JSON يخلي الـ JavaScript يحوله لصفحة الـ Login
            if (userId == null)
            {
                return Json(new
                {
                    success = false,
                    isNotLoggedIn = true,
                    redirectUrl = "/Account/Login"
                });
            }

            // التأكد إذا كان المنتج موجود في المفضلة فعلاً
            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.ItemID == itemId && f.UserID == userId);

            if (existingFavorite != null)
            {
                // لو موجود -> بنحذفه (Unfavorite)
                _context.Favorites.Remove(existingFavorite);
                await _context.SaveChangesAsync();
                return Json(new { success = true, status = "removed" });
            }
            else
            {
                // لو مش موجود -> بنضيفه (Favorite)
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

        // GET: Favorites (عرض قائمة المفضلات لليوزر الحالي)
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
                .ThenInclude(i => i.Images) // عشان لو عايزة تعرضي الصور في صفحة المفضلات
                .ToListAsync();

            return View(favorites);
        }

        // GET: Favorites/Delete/5 (للحذف التقليدي من صفحة المفضلات)
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