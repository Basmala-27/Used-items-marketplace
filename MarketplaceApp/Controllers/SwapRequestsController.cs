using MarketplaceApp.Data;
using MarketplaceApp.Enums;
using MarketplaceApp.Models;
using MarketplaceApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketplaceApp.Controllers
{
    [Authorize]
    public class SwapRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public SwapRequestsController(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // GET: SwapRequests/MyRequests — طلبات التبادل الواردة للمستخدم
        public async Task<IActionResult> MyRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var requests = await _context.SwapRequests
                .Include(s => s.OfferedItem).ThenInclude(i => i.Images)
                .Include(s => s.RequestedItem).ThenInclude(i => i.Images)
                .Include(s => s.Requester)
                .Where(s => s.RequestedItem.UserID == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return View(requests);
        }

        // GET: SwapRequests/MySentRequests — الطلبات التي أرسلها المستخدم
        public async Task<IActionResult> MySentRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var requests = await _context.SwapRequests
                .Include(s => s.OfferedItem).ThenInclude(i => i.Images)
                .Include(s => s.RequestedItem).ThenInclude(i => i.Images)
                .Include(s => s.Requester)
                .Where(s => s.RequesterId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return View(requests);
        }

        // =====================================================================
        //  POST: SwapRequests/Respond — قبول أو رفض طلب التبادل
        //  مع EF Transaction حقيقي لضمان تبادل الملكية بشكل آمن
        // =====================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(int id, OfferStatus status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var swapRequest = await _context.SwapRequests
                .Include(s => s.OfferedItem)
                .Include(s => s.RequestedItem)
                .Include(s => s.Requester)
                .FirstOrDefaultAsync(s => s.SwapRequestId == id);

            if (swapRequest == null)
                return Json(new { success = false, message = "الطلب غير موجود." });

            // التحقق أن المستجيب هو صاحب المنتج المطلوب
            if (swapRequest.RequestedItem.UserID != userId)
                return Json(new { success = false, message = "غير مصرح لك بالرد على هذا الطلب." });

            if (swapRequest.Status != OfferStatus.Pending)
                return Json(new { success = false, message = "هذا الطلب سبق معالجته." });

            if (status == OfferStatus.Rejected)
            {
                // رفض بسيط — لا يحتاج transaction معقد
                swapRequest.Status = OfferStatus.Rejected;
                await _context.SaveChangesAsync();

                // إشعار للمرسل
                await _notificationService.NotifySwapRequestRejectedAsync(swapRequest.RequesterId, swapRequest.SwapRequestId, swapRequest.RequestedItem.Title);

                TempData["Message"] = "تم رفض الطلب.";
                return RedirectToAction(nameof(MyRequests));
            }

            // ===========================================================
            // قبول الطلب — داخل EF Transaction لضمان الـ Atomicity
            // ===========================================================
            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                var offeredItemOwnerId  = swapRequest.OfferedItem.UserID;   // الشخص الطالب
                var requestedItemOwnerId = swapRequest.RequestedItem.UserID; // الشخص المستجيب

                // 1. تبادل الملكية
                swapRequest.OfferedItem.UserID  = requestedItemOwnerId;  // المنتج المعروض يصبح ملك المستجيب
                swapRequest.RequestedItem.UserID = offeredItemOwnerId;    // المنتج المطلوب يصبح ملك الطالب

                // 2. تغيير حالة المنتجين
                swapRequest.OfferedItem.Status  = ItemStatus.Swapped;
                swapRequest.RequestedItem.Status = ItemStatus.Swapped;

                // 3. تحديث حالة الطلب
                swapRequest.Status = OfferStatus.Accepted;

                // 4. سجل مالي للطرف الأول (الشخص الطالب)
                _context.Transactions.Add(new Transaction
                {
                    BuyerID       = offeredItemOwnerId,   // الطالب "أعطى" OfferedItem
                    SellerID      = requestedItemOwnerId,  // المستجيب "أعطى" RequestedItem
                    ItemID        = swapRequest.OfferedItemId,
                    FinalPrice    = swapRequest.OfferedItem.Price,
                    Status        = OrderStatus.Completed,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type          = TransactionType.SwapRecord,
                    Notes         = $"تبادل: أعطى \"{swapRequest.OfferedItem.Title}\" وأخذ \"{swapRequest.RequestedItem.Title}\"",
                    CreatedAt     = DateTime.Now
                });

                // 5. سجل مالي للطرف الثاني (الشخص المستجيب)
                _context.Transactions.Add(new Transaction
                {
                    BuyerID       = requestedItemOwnerId,  // المستجيب "أعطى" RequestedItem
                    SellerID      = offeredItemOwnerId,    // الطالب "أعطى" OfferedItem
                    ItemID        = swapRequest.RequestedItemId,
                    FinalPrice    = swapRequest.RequestedItem.Price,
                    Status        = OrderStatus.Completed,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type          = TransactionType.SwapRecord,
                    Notes         = $"تبادل: أعطى \"{swapRequest.RequestedItem.Title}\" وأخذ \"{swapRequest.OfferedItem.Title}\"",
                    CreatedAt     = DateTime.Now
                });

                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();

                // 6. إشعارات للطرفين
                await _notificationService.NotifySwapRequestAcceptedAsync(swapRequest.RequesterId, swapRequest.SwapRequestId, swapRequest.RequestedItem.Title);

                await _notificationService.SendAsync(
                    userId!,
                    NotificationType.Order,
                    swapRequest.SwapRequestId,
                    $"🔄 تم إتمام التبادل! \"{swapRequest.OfferedItem.Title}\" الآن ملكك.",
                    Url.Action("MyRequests", "SwapRequests")
                );

                TempData["Message"] = "تم قبول الطلب وإتمام التبادل بنجاح! 🎉";
                return RedirectToAction(nameof(MyRequests));
            }
            catch (Exception)
            {
                await dbTx.RollbackAsync();
                TempData["Error"] = "حدث خطأ أثناء معالجة التبادل. يرجى المحاولة مجدداً.";
                return RedirectToAction(nameof(MyRequests));
            }
        }

        // =====================================================================
        //  GET/POST: Index, Details, Create, Edit, Delete (CRUD بسيط)
        // =====================================================================

        public async Task<IActionResult> Index()
        {
            var swapRequests = await _context.SwapRequests
                .Include(s => s.Requester)
                .Include(s => s.OfferedItem)
                .Include(s => s.RequestedItem)
                .ToListAsync();
            return View(swapRequests);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var swapRequest = await _context.SwapRequests
                .Include(s => s.Requester)
                .Include(s => s.OfferedItem)
                .Include(s => s.RequestedItem)
                .FirstOrDefaultAsync(m => m.SwapRequestId == id);

            if (swapRequest == null) return NotFound();

            return View(swapRequest);
        }

        private bool SwapRequestExists(int id)
            => _context.SwapRequests.Any(e => e.SwapRequestId == id);
    }
}