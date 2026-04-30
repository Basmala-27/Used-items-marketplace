using MarketplaceApp.Data;
using MarketplaceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MarketplaceApp.Enums;
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

            // GET: Items/Create
            public IActionResult Create()
            {
                // تحضير قائمة الأقسام من الداتابيز
                ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name");

                // قائمة ثابتة للحالات (Condition) كما تظهر في الـ UI
                ViewBag.Conditions = new SelectList(new[] { "New", "Like New", "Used", "Refurbished" });

                return View();
            }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // مهم جداً نستخدم Include عشان بيانات الـ User والـ Category تظهر في الصفحة
            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.User)
                .Include(i => i.Images) // لو عاوزه تعرضي الصور برضه
                .FirstOrDefaultAsync(m => m.ItemID == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Create
        [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(ItemCreateViewModel vm)
            {
                if (ModelState.IsValid)
                {
                    // 1. تحويل الـ ViewModel إلى الـ Model الأصلي (Item)
                    var newItem = new Item
                    {
                        Title = vm.Title,
                        Description = vm.Description,
                        Price = vm.Price,
                        
                        Condition = Enum.Parse<ItemCondition>(vm.Condition.Replace(" ", "")),
                        CategoryID = vm.CategoryID,
                        Status = ItemStatus.Available, // القيمة الافتراضية
                        CreatedAt = DateTime.Now,
                        // هنا بنفترض إنك بتستخدمي Identity لجلب الـ UserID
                        // UserID = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                        UserID = User.FindFirstValue(ClaimTypes.NameIdentifier),// استبدليه بـ ID المستخدم الفعلي
                    };

                    // 2. معالجة رفع الصور
                    if (vm.ImageFiles != null && vm.ImageFiles.Count > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                        // التأكد من وجود الفولدر
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        foreach (var file in vm.ImageFiles)
                        {
                            // إنشاء اسم فريد للصورة لمنع التكرار
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            // حفظ الملف على السيرفر
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            // إضافة بيانات الصورة للـ Model الأصلي (ItemImage)
                            newItem.Images.Add(new ItemImage
                            {
                                ImageUrl = "/uploads/" + uniqueFileName
                            });
                        }
                    }

                    // 3. حفظ كل شيء في الداتابيز
                    _context.Add(newItem);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index)); // أو أي صفحة تانية
                }

                // لو فيه مشكلة في الـ Validation نرجع نملا الـ Dropdowns تاني
                ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name", vm.CategoryID);
                ViewBag.Conditions = new SelectList(new[] { "New", "Like New", "Used", "Refurbished" }, vm.Condition);

                return View(vm);
            }

            // Index لغرض التجربة
            public async Task<IActionResult> Index()
            {
                var items = await _context.Items.Include(i => i.Category).Include(i => i.Images).ToListAsync();
                return View(items);
            }
        }
    }