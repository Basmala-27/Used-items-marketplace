using MarketplaceApp.Data;
using MarketplaceApp.Enums;
using MarketplaceApp.Models;
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
    public class SwapRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SwapRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SwapRequests
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SwapRequests.Include(s => s.Requester);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SwapRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var swapRequest = await _context.SwapRequests
                .Include(s => s.Requester)
                .FirstOrDefaultAsync(m => m.SwapRequestId == id);
            if (swapRequest == null)
            {
                return NotFound();
            }

            return View(swapRequest);
        }

        // GET: SwapRequests/Create
        public IActionResult Create()
        {
            ViewData["RequesterId"] = new SelectList(_context.Users, "UserID", "Email");
            return View();
        }

        // POST: SwapRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SwapRequestId,RequesterId,OfferedItemId,RequestedItemId,Status,CreatedAt")] SwapRequest swapRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(swapRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RequesterId"] = new SelectList(_context.Users, "UserID", "Email", swapRequest.RequesterId);
            return View(swapRequest);
        }

        // GET: SwapRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var swapRequest = await _context.SwapRequests.FindAsync(id);
            if (swapRequest == null)
            {
                return NotFound();
            }
            ViewData["RequesterId"] = new SelectList(_context.Users, "UserID", "Email", swapRequest.RequesterId);
            return View(swapRequest);
        }

        // POST: SwapRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SwapRequestId,RequesterId,OfferedItemId,RequestedItemId,Status,CreatedAt")] SwapRequest swapRequest)
        {
            if (id != swapRequest.SwapRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(swapRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SwapRequestExists(swapRequest.SwapRequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RequesterId"] = new SelectList(_context.Users, "UserID", "Email", swapRequest.RequesterId);
            return View(swapRequest);
        }

        // GET: SwapRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var swapRequest = await _context.SwapRequests
                .Include(s => s.Requester)
                .FirstOrDefaultAsync(m => m.SwapRequestId == id);
            if (swapRequest == null)
            {
                return NotFound();
            }

            return View(swapRequest);
        }

        // POST: SwapRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var swapRequest = await _context.SwapRequests.FindAsync(id);
            if (swapRequest != null)
            {
                _context.SwapRequests.Remove(swapRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SwapRequestExists(int id)
        {
            return _context.SwapRequests.Any(e => e.SwapRequestId == id);
        }

        public async Task<IActionResult> MyRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var requests = await _context.SwapRequests
         .Include(s => s.OfferedItem)   // عشان يجيب بيانات المنتج المعروض
         .Include(s => s.RequestedItem) // عشان يجيب بيانات منتجك
         .Include(s => s.Requester)     // مهم جداً: عشان يجيب بيانات الشخص (Email/UserName)
         .Where(s => s.RequestedItem.UserID == userId)
         .ToListAsync();

            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(int id, OfferStatus status)
        {
            var swapRequest = await _context.SwapRequests
                .Include(s => s.OfferedItem)
                .Include(s => s.RequestedItem)
                .FirstOrDefaultAsync(s => s.SwapRequestId == id);

            if (swapRequest == null) return NotFound();

            // تحديث حالة الطلب
            swapRequest.Status = status;

            if (status == OfferStatus.Accepted)
            {
                // لو وافقنا، بنخلي المنتجين غير متاحين للبيع مرة تانية
                swapRequest.OfferedItem.Status = ItemStatus.Sold; // أو Exchanged
                swapRequest.RequestedItem.Status = ItemStatus.Sold;
            }

            _context.Update(swapRequest);
            await _context.SaveChangesAsync();

            TempData["Message"] = status == OfferStatus.Accepted ? "تم قبول الطلب بنجاح!" : "تم رفض الطلب.";

            return RedirectToAction(nameof(MyRequests));
        }
    }
}