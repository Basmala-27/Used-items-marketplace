using MarketplaceApp.Data;
using MarketplaceApp.Models;
using MarketplaceApp.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketplaceApp.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        //newItem = new Item
        // GET: Items/Index
        public async Task<IActionResult> Index(int? categoryId, string? condition, string? listingType, string? sort)
        {
            var query = _context.Items
                .Include(i => i.Category)
                .Include(i => i.User)
                .Include(i => i.Images)
                .AsQueryable();

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
             
                "price_asc" => query.OrderBy(i => (double)i.Price),
                "price_desc" => query.OrderByDescending(i => (double)i.Price),
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

            if (categoryId.HasValue)
            {
                var category = await _context.Categories.FindAsync(categoryId.Value);
                ViewBag.CategoryName = category?.Name;
            }

            return View(items);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.User)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(m => m.ItemID == id);

            if (item == null) return NotFound();

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name");
            ViewBag.Conditions = new SelectList(new[] { "Like New", "Very Good", "Good", "Needs Repair" });
            ViewBag.ListingTypes = new SelectList(new[] { "Sale", "Swap" });
            return View();
        }

        // POST: Items/Create
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
                    Status = ItemStatus.Available, // FIXED: enum instead of string
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
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        newItem.Images.Add(new ItemImage
                        {
                            ImageUrl = "/uploads/" + uniqueFileName
                        });
                    }
                }

                _context.Add(newItem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name", vm.CategoryID);
            ViewBag.Conditions = new SelectList(new[] { "Like New", "Very Good", "Good", "Needs Repair" }, vm.Condition);


            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSwap(int targetItemId)
        {
            var targetItem = await _context.Items.FindAsync(targetItemId);
            if (targetItem == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 1. هنجيب كل منتجاتك بدون أي شروط عشان نتأكد إن الـ UserID صح
            var allMyItems = await _context.Items
                .Where(i => i.UserID == currentUserId)
                .ToListAsync();

            // 2. هنجيب المنتجات اللي بتحقق الشرط بس (سعر + متاح)
            decimal priceTolerance = targetItem.Price * 0.20m;
            decimal minPrice = targetItem.Price - priceTolerance;
            decimal maxPrice = targetItem.Price + priceTolerance;

            var filteredItems = allMyItems
                .Where(i => i.Status == ItemStatus.Available &&
                            i.Price >= minPrice &&
                            i.Price <= maxPrice)
                .ToList();

            // نبعت البيانات للـ View
            ViewBag.TotalMyItems = allMyItems.Count; // عدد منتجاتك الكلي
            ViewBag.MinAllowedPrice = minPrice;
            ViewBag.MaxAllowedPrice = maxPrice;

            var viewModel = new CreateSwapViewModel
            {
                RequestedItemId = targetItem.ItemID,
                RequestedItemName = targetItem.Title,
                RequestedItemPrice = targetItem.Price,
                MyAvailableItems = filteredItems // اللي هيظهر في الـ Dropdown
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSwap(CreateSwapViewModel model)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                var swapRequest = new SwapRequest
                {
                    RequesterId = currentUserId,
                    OfferedItemId = model.OfferedItemId,
                    RequestedItemId = model.RequestedItemId,
                    Status = OfferStatus.Pending,
                    CreatedAt = DateTime.Now
                };

                _context.SwapRequests.Add(swapRequest);
                await _context.SaveChangesAsync();

                TempData["Success"] = "تم إرسال طلب التبادل بنجاح!";
                return RedirectToAction("Details", new { id = model.RequestedItemId });
            }

            // --- لو وصلنا هنا يبقى فيه خطأ (Validation Error) ---

            // 1. اجمعي كل الأخطاء وحطيها في الـ ViewBag عشان تظهر في الصفحة
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            ViewBag.ValidationErrors = errors;

            // 2. لازم نعيد تحميل البيانات عشان الـ Dropdown ما تضربش
            var targetItem = await _context.Items.FindAsync(model.RequestedItemId);
            model.RequestedItemName = targetItem?.Title ?? "Unknown";
            model.RequestedItemPrice = targetItem?.Price ?? 0;

            model.MyAvailableItems = await _context.Items
                .Where(i => i.UserID == currentUserId && i.Status == ItemStatus.Available)
                .ToListAsync();

            return View(model);
        }

    }
}