using MarketplaceApp.Data;
using MarketplaceApp.Enums;
using MarketplaceApp.Models;
using MarketplaceApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketplaceApp.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Services.INotificationService _notificationService;

        public ItemsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, Services.INotificationService notificationService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _notificationService = notificationService;
        }

        // ===================== INDEX =====================
        public async Task<IActionResult> Index(int? categoryId, string? condition, string? listingType, string? sort, string? searchTerm)
        {
            var query = _context.Items
                .Include(i => i.Category)
                .Include(i => i.User)
                .Include(i => i.Images)
                .Where(i => i.Status == ItemStatus.Available)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var normalizedSearch = searchTerm.Trim().ToLower();
                query = query.Where(i => i.Title.ToLower().Contains(normalizedSearch) ||
                                         i.Description.ToLower().Contains(normalizedSearch));
            }

            if (categoryId.HasValue)
                query = query.Where(i => i.CategoryID == categoryId.Value);

            if (!string.IsNullOrEmpty(condition) && condition != "all")
                query = query.Where(i => i.Condition == condition);

            if (!string.IsNullOrEmpty(listingType) && listingType != "all")
            {
                if (listingType == "Swap")
                    query = query.Where(i => i.IsAvailableForSwap);
                else if (listingType == "Sale")
                    query = query.Where(i => i.IsAvailableForSale);
            }

            query = sort switch
            {
                "price_asc" => query.OrderBy(i => i.Price),
                "price_desc" => query.OrderByDescending(i => i.Price),
                _ => query.OrderByDescending(i => i.CreatedAt)
            };

            var items = await query.ToListAsync();

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name", categoryId);
            ViewBag.Conditions = new SelectList(new[] { "Like New", "Very Good", "Good", "Needs Repair" }, condition);
            ViewBag.ListingTypes = new SelectList(new[] { "Sale", "Swap" }, listingType);

            ViewBag.SelectedCategory = categoryId;
            ViewBag.SelectedCondition = condition;
            ViewBag.SelectedListingType = listingType;
            ViewBag.SelectedSort = sort;
            ViewBag.SearchTerm = searchTerm;

            return View(items);
        }

        // ===================== DETAILS =====================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.User)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(m => m.ItemID == id);

            if (item == null) return NotFound();

            // Privacy Protection Logic
            if (item.Status != ItemStatus.Available)
            {
                bool canAccess = false;
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                if (User.IsInRole("Admin") || item.UserID == currentUserId)
                {
                    canAccess = true;
                }
                else if (!string.IsNullOrEmpty(currentUserId))
                {
                    bool isTransactionBuyer = await _context.Transactions
                        .AnyAsync(t => t.ItemID == item.ItemID && t.BuyerID == currentUserId);

                    bool isAcceptedBuyerRequest = await _context.BuyRequests
                        .AnyAsync(br => br.ItemID == item.ItemID && br.BuyerID == currentUserId && br.Status == BuyRequestStatus.SellerAccepted);

                    if (isTransactionBuyer || isAcceptedBuyerRequest)
                    {
                        canAccess = true;
                    }
                }

                if (!canAccess)
                {
                    return View("ItemUnavailable");
                }
            }

            return View(item);
        }

        // ===================== CREATE =====================
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name");
            ViewBag.Conditions = new SelectList(new[] { "Like New", "Very Good", "Good", "Needs Repair" });
            ViewBag.ListingTypes = new SelectList(new[] { "Sale", "Swap" });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newItem = new Item
                {
                    Title = vm.Title,
                    Description = vm.Description,
                    Price = vm.Price,
                    Condition = vm.Condition,
                    Location = vm.Location,
                    CategoryID = vm.CategoryID,
                    IsAvailableForSale = vm.IsAvailableForSale,
                    IsAvailableForSwap = vm.IsAvailableForSwap,
                    Status = ItemStatus.Available,
                    CreatedAt = DateTime.Now,
                    UserID = User.FindFirstValue(ClaimTypes.NameIdentifier),
                };

                if (vm.ImageFiles != null && vm.ImageFiles.Count > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    foreach (var file in vm.ImageFiles)
                    {
                        string uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        newItem.Images.Add(new ItemImage { ImageUrl = "/uploads/" + uniqueFileName });
                    }
                }

                _context.Add(newItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(vm);
        }

        // ===================== EDIT =====================
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemID == id);
            if (item == null) return NotFound();

            var vm = new ItemEditViewModel
            {
                ItemID = item.ItemID,
                Title = item.Title,
                Description = item.Description,
                Price = item.Price,
                Condition = item.Condition,
                Location = item.Location,
                Status = item.Status,
                CategoryID = item.CategoryID,
                UserID = item.UserID
            };

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name", vm.CategoryID);
            ViewBag.Conditions = new SelectList(new[] { "Like New", "Very Good", "Good", "Needs Repair" }, vm.Condition);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemEditViewModel vm)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemID == vm.ItemID);
            if (item == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name", vm.CategoryID);
                ViewBag.Conditions = new SelectList(new[] { "Like New", "Very Good", "Good", "Needs Repair" }, vm.Condition);

                return View(vm);
            }

            item.Title = vm.Title;
            item.Description = vm.Description;
            item.Price = vm.Price;
            item.Condition = vm.Condition;
            item.Location = vm.Location;
            item.Status = vm.Status;
            item.CategoryID = vm.CategoryID;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        // ===================== DELETE =====================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Items
                .Include(i => i.Category)
                .FirstOrDefaultAsync(i => i.ItemID == id);

            if (item == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (item.UserID != userId) return Unauthorized();

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Include all navigation properties that could block deletion
            var item = await _context.Items
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.ItemID == id);

            if (item == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (item.UserID != userId) return Unauthorized();

            // 1. Delete Physical Image Files from the Server
            if (item.Images != null)
            {
                foreach (var img in item.Images)
                {
                    var relativePath = img.ImageUrl?.TrimStart('/');
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath ?? "");
                    if (!string.IsNullOrEmpty(relativePath) && System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            // 2. Handle Related Records Manually (Insurance for SQLite)

            // Remove Favorites linked to this item
            var favorites = _context.Favorites.Where(f => f.ItemID == id);
            _context.Favorites.RemoveRange(favorites);

            // Remove Conversations (and their Messages via Cascade) linked to this item
            var conversations = _context.Conversations.Where(c => c.ItemID == id);
            _context.Conversations.RemoveRange(conversations);

            // Remove BuyRequests linked to this item
            var buyRequests = _context.BuyRequests.Where(b => b.ItemID == id);
            _context.BuyRequests.RemoveRange(buyRequests);

            // 3. Finally, remove the item
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Item and all related data deleted successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}