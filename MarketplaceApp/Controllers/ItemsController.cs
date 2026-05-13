using MarketplaceApp.Data;
using MarketplaceApp.Enums;
using MarketplaceApp.Models;
using MarketplaceApp.ViewModels;
using MarketplaceApp.Services; 
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
        private readonly INotificationService _notificationService;

        public ItemsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, INotificationService notificationService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _notificationService = notificationService;
        }

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
                "price_asc" => query.OrderBy(i => i.PriceSortValue),

                "price_desc" => query.OrderByDescending(i => i.PriceSortValue),

                _ => query.OrderByDescending(i => i.CreatedAt)
            };

            var items = await query.ToListAsync();



            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name", categoryId);
            ViewBag.Conditions = new SelectList(new[] { "Like New", "Very Good", "Good", "Needs Repair" }, condition);
            ViewBag.ListingTypes = new SelectList(new[] { "Sale", "Swap" }, listingType);
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SelectedSort = sort;
            ViewBag.SearchTerm = searchTerm;

            if (categoryId.HasValue)
            {
                var category = await _context.Categories.FindAsync(categoryId.Value);
                ViewBag.CategoryName = category?.Name;
            }

            return View(items);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.User)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(m => m.ItemID == id);

            if (item == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (item.Status != ItemStatus.Available)
            {
                bool canAccess = false;
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

                    if (isTransactionBuyer || isAcceptedBuyerRequest) canAccess = true;
                }

                if (!canAccess) return View("ItemUnavailable");
            }

            if (User.Identity?.IsAuthenticated == true)
            {
                var currentUser = await _context.Users.FindAsync(currentUserId);
                ViewBag.WalletBalance = currentUser?.WalletBalance ?? 0;
                ViewBag.PendingBalance = currentUser?.PendingBalance ?? 0;
            }

            return View(item);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name");
            ViewBag.Conditions = new SelectList(new[] { "Like New", "Very Good", "Good", "Needs Repair" });
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
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    foreach (var file in vm.ImageFiles)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create)) { await file.CopyToAsync(stream); }
                        newItem.Images.Add(new ItemImage { ImageUrl = "/uploads/" + uniqueFileName });
                    }
                }

                _context.Add(newItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemID == id);
            if (item == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (item.UserID != currentUserId) return Unauthorized();

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

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (item.UserID != currentUserId) return Unauthorized();

            if (ModelState.IsValid)
            {
                item.Title = vm.Title;
                item.Description = vm.Description;
                item.Price = vm.Price;
                item.Condition = vm.Condition;
                item.Location = vm.Location;
                item.Status = vm.Status;
                item.CategoryID = vm.CategoryID;

                try
                {
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Item updated successfully!";
                    return RedirectToAction("Details", new { id = item.ItemID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again.");
                }
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name", vm.CategoryID);
            ViewBag.Conditions = new SelectList(new[] { "Like New", "Very Good", "Good", "Needs Repair" }, vm.Condition);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.ItemID == id);

            if (item == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (item.UserID != currentUserId) return Unauthorized();

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ItemID) 
        {
            var item = await _context.Items
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.ItemID == ItemID);

            if (item == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (item.UserID != userId) return Unauthorized();

            // 1. ??? ??????? ?????????? (?????)
            if (item.Images != null)
            {
                foreach (var img in item.Images)
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, img.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                }
            }

            _context.Favorites.RemoveRange(_context.Favorites.Where(f => f.ItemID == ItemID));
            _context.Conversations.RemoveRange(_context.Conversations.Where(c => c.ItemID == ItemID));
            _context.BuyRequests.RemoveRange(_context.BuyRequests.Where(b => b.ItemID == ItemID));

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Item deleted successfully.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> CreateSwap(int targetItemId)
        {
            var targetItem = await _context.Items.FindAsync(targetItemId);
            if (targetItem == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            decimal priceTolerance = targetItem.Price * 0.20m;

            ViewBag.MinAllowedPrice = targetItem.Price - priceTolerance;
            ViewBag.MaxAllowedPrice = targetItem.Price + priceTolerance;

            var filteredItems = await _context.Items
                .Where(i => i.UserID == currentUserId && i.Status == ItemStatus.Available &&
                            i.Price >= (targetItem.Price - priceTolerance) &&
                            i.Price <= (targetItem.Price + priceTolerance))
                .ToListAsync();

            var viewModel = new CreateSwapViewModel
            {
                RequestedItemId = targetItem.ItemID,
                RequestedItemName = targetItem.Title,
                RequestedItemPrice = targetItem.Price,
                MyAvailableItems = filteredItems
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSwap(CreateSwapViewModel model)
        {
            if (ModelState.IsValid)
            {
                var swapRequest = new SwapRequest
                {
                    RequesterId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    OfferedItemId = model.OfferedItemId,
                    RequestedItemId = model.RequestedItemId,
                    Status = SwapRequestStatus.Pending,
                    CreatedAt = DateTime.Now
                };

                _context.SwapRequests.Add(swapRequest);
                await _context.SaveChangesAsync();

                var requestedItem = await _context.Items.FindAsync(model.RequestedItemId);
                if (requestedItem != null)
                {
                    await _notificationService.NotifySwapRequestAsync(
                        requestedItem.UserID, 
                        swapRequest.SwapRequestId, 
                        requestedItem.Title
                    );
                }

                TempData["Success"] = "Swap request sent successfully!";
                return RedirectToAction("Details", new { id = model.RequestedItemId });
            }
            return View(model);
        }
    }
}