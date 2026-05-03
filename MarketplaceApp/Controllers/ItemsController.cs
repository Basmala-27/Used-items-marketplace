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

        // GET: Items/Index
        public async Task<IActionResult> Index(int? categoryId, string? condition, string? listingType, string? sort, string? searchTerm)
        {
            var query = _context.Items
                .Include(i => i.Category)
                .Include(i => i.User)
                .Include(i => i.Images)
                .AsQueryable();

            // 1. Search Logic
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(i => i.Title!.Contains(searchTerm) || i.Description!.Contains(searchTerm));
            }

            // 2. Category Filter
            if (categoryId.HasValue)
            {
                query = query.Where(i => i.CategoryID == categoryId.Value);
                var category = await _context.Categories.FindAsync(categoryId.Value);
                ViewBag.CategoryName = category?.Name;
            }

            // 3. Condition Filter
            if (!string.IsNullOrEmpty(condition) && condition != "all")
            {
                query = query.Where(i => i.Condition == condition);
            }

            // 4. Listing Type Filter
            if (!string.IsNullOrEmpty(listingType) && listingType != "all")
            {
                query = query.Where(i => i.ListingType == listingType);
            }

            // --- THE FIX STARTS HERE ---

            // First, execute the filtered query to get the items into memory (List)
            // This avoids the SQLite "decimal" sorting exception.
            var itemsList = await query.ToListAsync();

            // Now, apply the sorting to the List in memory
            itemsList = sort switch
            {
                "price_asc" => itemsList.OrderBy(i => i.Price).ToList(),
                "price_desc" => itemsList.OrderByDescending(i => i.Price).ToList(),
                _ => itemsList.OrderByDescending(i => i.CreatedAt).ToList()
            };

            // --- THE FIX ENDS HERE ---

            // Keep UI values in sync
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SelectedCondition = condition;
            ViewBag.SelectedListingType = listingType;
            ViewBag.SelectedSort = sort;

            // Repopulate SelectLists
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name", categoryId);
            ViewBag.Conditions = new SelectList(new[] { "Like New", "Very Good", "Good", "Needs Repair" }, condition);
            ViewBag.ListingTypes = new SelectList(new[] { "Sale", "Swap" }, listingType);

            return View(itemsList);
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
                    ListingType = vm.ListingType,
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
            ViewBag.ListingTypes = new SelectList(new[] { "Sale", "Swap" }, vm.ListingType);

            return View(vm);
        }
    }
}