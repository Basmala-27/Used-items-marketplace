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
        private readonly Services.INotificationService _notificationService;

        public ItemsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, Services.INotificationService notificationService)
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

            // جلب رصيد المحفظة للمستخدم الحالي (للـ Buy Modal)
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentUser = await _context.Users.FindAsync(userId);
                ViewBag.WalletBalance = currentUser?.WalletBalance ?? 0;
                ViewBag.PendingBalance = currentUser?.PendingBalance ?? 0;
            }

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

                var requestedItem = await _context.Items.FindAsync(model.RequestedItemId);
                if (requestedItem != null)
                {
                    await _notificationService.NotifySwapRequestAsync(requestedItem.UserID, swapRequest.SwapRequestId, requestedItem.Title);
                }

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

        // ===================== EDIT =====================

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            // نجيب الـ item مع الكاتيجوري (أحسن من FindAsync بس)
            var item = await _context.Items
                .Include(i => i.Category)
                .FirstOrDefaultAsync(i => i.ItemID == id);

            if (item == null)
                return NotFound();

            // تأكد إن اليوزر هو صاحب الـ item
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (item.UserID != userId)
                return Unauthorized();

            // تجهيز الـ dropdown
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name", item.CategoryID);

            return View(item);
        }


        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Item item)
        {
            if (id != item.ItemID)
                return NotFound();

            // نجيب الأصل من الداتابيز (عشان نحافظ على الداتا المهمة)
            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.ItemID == id);

            if (existingItem == null)
                return NotFound();

            // تأكد من الملكية
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (existingItem.UserID != userId)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                // نحافظ على الداتا اللي مينفعش تتغير
                item.UserID = existingItem.UserID;
                item.CreatedAt = existingItem.CreatedAt;

                // مهم: نجيب الـ Images القديمة عشان ما تتشالش بالغلط
                item.Images = existingItem.Images;

                _context.Update(item);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name", item.CategoryID);
            return View(item);
        }

    }
}