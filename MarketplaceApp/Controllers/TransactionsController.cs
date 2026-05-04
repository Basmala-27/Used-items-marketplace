using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Data;
using MarketplaceApp.Enums;
using MarketplaceApp.Models;
using MarketplaceApp.Services;

namespace MarketplaceApp.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public TransactionsController(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // =====================================================================
        //  GET: Transactions/Index
        // =====================================================================
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transactions
                .Include(t => t.Buyer)
                .Include(t => t.Item)
                .Include(t => t.Seller)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            return View(transactions);
        }

        // =====================================================================
        //  GET: Transactions/MyOrders — طلبات المستخدم كمشترٍ
        // =====================================================================
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var buyRequests = await _context.BuyRequests
                .Include(b => b.Item).ThenInclude(i => i.Images)
                .Include(b => b.Seller)
                .Where(b => b.BuyerID == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            var user = await _context.Users.FindAsync(userId);
            ViewBag.WalletBalance  = user?.WalletBalance  ?? 0;
            ViewBag.PendingBalance = user?.PendingBalance ?? 0;

            return View(buyRequests);
        }

        // =====================================================================
        //  GET: Transactions/SellerRequests — الطلبات الواردة للبائع
        // =====================================================================
        public async Task<IActionResult> SellerRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Buy Requests للبائع
            var buyRequests = await _context.BuyRequests
                .Include(b => b.Item).ThenInclude(i => i.Images)
                .Include(b => b.Buyer)
                .Where(b => b.SellerID == userId &&
                            (b.Status == BuyRequestStatus.Pending || b.Status == BuyRequestStatus.SellerAccepted))
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            // Swap Requests للبائع
            var swapRequests = await _context.SwapRequests
                .Include(s => s.OfferedItem).ThenInclude(i => i.Images)
                .Include(s => s.RequestedItem).ThenInclude(i => i.Images)
                .Include(s => s.Requester)
                .Where(s => s.RequestedItem.UserID == userId && s.Status == OfferStatus.Pending)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            ViewBag.BuyRequests  = buyRequests;
            ViewBag.SwapRequests = swapRequests;

            return View();
        }

        // =====================================================================
        //  GET: Transactions/Details/5
        // =====================================================================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var transaction = await _context.Transactions
                .Include(t => t.Buyer)
                .Include(t => t.Item)
                .Include(t => t.Seller)
                .FirstOrDefaultAsync(m => m.TransactionID == id);

            if (transaction == null) return NotFound();

            return View(transaction);
        }

        // =====================================================================
        //  POST: Transactions/BuyItem
        //  المشتري يضغط Buy → خصم 100% → Reserved → BuyRequest (Pending)
        // =====================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyItem(int itemId)
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
                return Json(new { success = false, message = "المنتج غير موجود." });

            if (item.UserID == buyerId)
                return Json(new { success = false, message = "لا يمكنك شراء منتجك الخاص." });

            if (item.Status != ItemStatus.Available)
                return Json(new { success = false, message = "هذا المنتج غير متاح للشراء حالياً." });

            var buyer = await _context.Users.FindAsync(buyerId);
            if (buyer == null)
                return Json(new { success = false, message = "لم يتم العثور على المستخدم." });

            if (buyer.WalletBalance < item.Price)
                return Json(new { success = false, message = $"رصيدك غير كافٍ. رصيدك: ${buyer.WalletBalance:0.00} | السعر: ${item.Price:0.00}" });

            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. خصم المبلغ كاملاً وحفظه في Escrow
                buyer.WalletBalance  -= item.Price;
                buyer.PendingBalance += item.Price;

                // 2. حجز المنتج (لا يظهر لأحد آخر)
                item.Status = ItemStatus.Reserved;

                // 3. سجل مالي — حجز Escrow
                var escrowTx = new Transaction
                {
                    BuyerID       = buyerId!,
                    SellerID      = item.UserID,
                    ItemID        = item.ItemID,
                    FinalPrice    = item.Price,
                    Status        = OrderStatus.Pending,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type          = TransactionType.Purchase,
                    Notes         = $"حجز Escrow كامل — {item.Title} — بانتظار قبول البائع",
                    CreatedAt     = DateTime.Now
                };
                _context.Transactions.Add(escrowTx);
                await _context.SaveChangesAsync(); // نحتاج الـ ID

                // 4. إنشاء BuyRequest
                var buyRequest = new BuyRequest
                {
                    BuyerID            = buyerId!,
                    SellerID           = item.UserID,
                    ItemID             = item.ItemID,
                    Amount             = item.Price,
                    Status             = BuyRequestStatus.Pending,
                    EscrowTransactionId = escrowTx.TransactionID,
                    CreatedAt          = DateTime.Now
                };
                _context.BuyRequests.Add(buyRequest);
                await _context.SaveChangesAsync();

                await dbTx.CommitAsync();

                // 5. إشعار للبائع
                await _notificationService.SendAsync(
                    item.UserID,
                    NotificationType.Order,
                    buyRequest.BuyRequestId,
                    $"🛒 طلب شراء جديد لمنتجك \"{item.Title}\" بسعر ${item.Price:0.00} — قبل أو ارفض الطلب",
                    Url.Action("SellerRequests", "Transactions")
                );

                return Json(new { success = true, message = "تم إرسال طلب الشراء! في انتظار موافقة البائع. 🎉" });
            }
            catch (Exception)
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "حدث خطأ. يرجى المحاولة لاحقاً." });
            }
        }

        // =====================================================================
        //  POST: Transactions/SellerAcceptBuy — البائع يقبل الطلب → 50% فوراً
        // =====================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SellerAcceptBuy(int buyRequestId)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var buyRequest = await _context.BuyRequests
                .Include(b => b.Item)
                .Include(b => b.Buyer)
                .FirstOrDefaultAsync(b => b.BuyRequestId == buyRequestId);

            if (buyRequest == null)
                return Json(new { success = false, message = "الطلب غير موجود." });

            if (buyRequest.SellerID != sellerId)
                return Json(new { success = false, message = "غير مصرح لك." });

            if (buyRequest.Status != BuyRequestStatus.Pending)
                return Json(new { success = false, message = "لا يمكن قبول هذا الطلب." });

            var buyer  = await _context.Users.FindAsync(buyRequest.BuyerID);
            var seller = await _context.Users.FindAsync(sellerId);
            if (buyer == null || seller == null)
                return Json(new { success = false, message = "بيانات المستخدمين غير صحيحة." });

            var halfAmount = Math.Round(buyRequest.Amount / 2, 2);

            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. تحويل 50% من Escrow للبائع
                buyer.PendingBalance  -= halfAmount;
                seller.WalletBalance  += halfAmount;

                // 2. تحديث حالات
                buyRequest.Status      = BuyRequestStatus.SellerAccepted;
                buyRequest.UpdatedAt   = DateTime.Now;
                buyRequest.Item.Status = ItemStatus.PendingDelivery;

                // 3. سجل مالي — الدفعة الأولى (50%)
                _context.Transactions.Add(new Transaction
                {
                    BuyerID       = buyRequest.BuyerID,
                    SellerID      = sellerId!,
                    ItemID        = buyRequest.ItemID,
                    FinalPrice    = halfAmount,
                    Status        = OrderStatus.PartiallyPaid,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type          = TransactionType.PartialPayment,
                    Notes         = $"دفعة أولى (50%) — {buyRequest.Item.Title} — بعد قبول البائع",
                    CreatedAt     = DateTime.Now
                });

                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();

                // 4. إشعار للمشتري
                await _notificationService.SendAsync(
                    buyRequest.BuyerID,
                    NotificationType.Order,
                    buyRequest.BuyRequestId,
                    $"✅ البائع قبل طلب شرائك لـ \"{buyRequest.Item.Title}\"! جارٍ الشحن. أكّد استلامك لتحرير المبلغ.",
                    Url.Action("MyOrders", "Transactions")
                );

                return Json(new { success = true, message = $"قبلت الطلب وتم تحويل ${halfAmount:0.00} (50%) لمحفظتك فوراً! ✅" });
            }
            catch (Exception)
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "حدث خطأ. يرجى المحاولة لاحقاً." });
            }
        }

        // =====================================================================
        //  POST: Transactions/SellerRejectBuy — البائع يرفض → استرداد كامل
        // =====================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SellerRejectBuy(int buyRequestId)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var buyRequest = await _context.BuyRequests
                .Include(b => b.Item)
                .FirstOrDefaultAsync(b => b.BuyRequestId == buyRequestId);

            if (buyRequest == null)
                return Json(new { success = false, message = "الطلب غير موجود." });

            if (buyRequest.SellerID != sellerId)
                return Json(new { success = false, message = "غير مصرح لك." });

            if (buyRequest.Status != BuyRequestStatus.Pending)
                return Json(new { success = false, message = "لا يمكن رفض هذا الطلب." });

            var buyer = await _context.Users.FindAsync(buyRequest.BuyerID);
            if (buyer == null)
                return Json(new { success = false, message = "بيانات المشتري غير صحيحة." });

            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. إعادة المبلغ كاملاً للمشتري
                buyer.PendingBalance -= buyRequest.Amount;
                buyer.WalletBalance  += buyRequest.Amount;

                // 2. تحرير المنتج (يعود للبيع)
                buyRequest.Item.Status = ItemStatus.Available;
                buyRequest.Status      = BuyRequestStatus.RejectedBySeller;
                buyRequest.UpdatedAt   = DateTime.Now;

                // 3. سجل مالي — استرداد
                _context.Transactions.Add(new Transaction
                {
                    BuyerID       = buyRequest.BuyerID,
                    SellerID      = sellerId!,
                    ItemID        = buyRequest.ItemID,
                    FinalPrice    = buyRequest.Amount,
                    Status        = OrderStatus.RejectedBySeller,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type          = TransactionType.Refund,
                    Notes         = $"استرداد كامل — رفض البائع طلب شراء \"{buyRequest.Item.Title}\"",
                    CreatedAt     = DateTime.Now
                });

                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();

                // 4. إشعار للمشتري
                await _notificationService.SendAsync(
                    buyRequest.BuyerID,
                    NotificationType.Order,
                    buyRequest.BuyRequestId,
                    $"❌ رفض البائع طلب شرائك لـ \"{buyRequest.Item.Title}\". تم استرداد ${buyRequest.Amount:0.00} لمحفظتك.",
                    Url.Action("MyOrders", "Transactions")
                );

                return Json(new { success = true, message = "تم رفض الطلب. رُدّ المبلغ كاملاً للمشتري." });
            }
            catch (Exception)
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "حدث خطأ. يرجى المحاولة لاحقاً." });
            }
        }

        // =====================================================================
        //  POST: Transactions/ConfirmDelivery
        //  المشتري يؤكد الاستلام → 50% الثانية للبائع + Sold + نقل الملكية
        // =====================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelivery(int buyRequestId)
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var buyRequest = await _context.BuyRequests
                .Include(b => b.Item)
                .FirstOrDefaultAsync(b => b.BuyRequestId == buyRequestId);

            if (buyRequest == null)
                return Json(new { success = false, message = "الطلب غير موجود." });

            if (buyRequest.BuyerID != buyerId)
                return Json(new { success = false, message = "غير مصرح لك." });

            if (buyRequest.Status != BuyRequestStatus.SellerAccepted)
                return Json(new { success = false, message = "لا يمكن تأكيد الاستلام في هذه المرحلة." });

            var buyer  = await _context.Users.FindAsync(buyerId);
            var seller = await _context.Users.FindAsync(buyRequest.SellerID);
            if (buyer == null || seller == null)
                return Json(new { success = false, message = "بيانات المستخدمين غير صحيحة." });

            // الـ 50% الثانية (ما تبقى في PendingBalance)
            var remainingAmount = Math.Round(buyRequest.Amount / 2, 2);

            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. تحويل الـ 50% الثانية من Escrow للبائع
                buyer.PendingBalance  -= remainingAmount;
                seller.WalletBalance  += remainingAmount;

                // 2. تحديث حالة المنتج + نقل الملكية ✅
                buyRequest.Item.Status = ItemStatus.Sold;
                buyRequest.Item.UserID = buyerId!;   // نقل الملكية للمشتري

                // 3. إغلاق BuyRequest
                buyRequest.Status    = BuyRequestStatus.Completed;
                buyRequest.UpdatedAt = DateTime.Now;

                // 4. سجل مالي — الدفعة الأخيرة (50%)
                _context.Transactions.Add(new Transaction
                {
                    BuyerID       = buyerId!,
                    SellerID      = buyRequest.SellerID,
                    ItemID        = buyRequest.ItemID,
                    FinalPrice    = remainingAmount,
                    Status        = OrderStatus.Completed,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type          = TransactionType.FinalPayment,
                    Notes         = $"دفعة أخيرة (50%) — {buyRequest.Item.Title} — بعد تأكيد الاستلام",
                    CreatedAt     = DateTime.Now
                });

                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();

                // 5. إشعار للبائع
                await _notificationService.SendAsync(
                    buyRequest.SellerID,
                    NotificationType.Order,
                    buyRequest.BuyRequestId,
                    $"✅ أكد المشتري استلام \"{buyRequest.Item.Title}\". تم تحويل ${remainingAmount:0.00} (50%) الأخيرة لمحفظتك!",
                    Url.Action("Index", "Transactions")
                );

                return Json(new { success = true, message = $"تم تأكيد الاستلام! تم تحويل ${remainingAmount:0.00} للبائع. المنتج الآن ملكك! 🎉" });
            }
            catch (Exception)
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "حدث خطأ. يرجى المحاولة لاحقاً." });
            }
        }

        // =====================================================================
        //  POST: Transactions/CancelBuyRequest — المشتري يلغي قبل قبول البائع
        // =====================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelBuyRequest(int buyRequestId)
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var buyRequest = await _context.BuyRequests
                .Include(b => b.Item)
                .FirstOrDefaultAsync(b => b.BuyRequestId == buyRequestId);

            if (buyRequest == null)
                return Json(new { success = false, message = "الطلب غير موجود." });

            if (buyRequest.BuyerID != buyerId)
                return Json(new { success = false, message = "غير مصرح لك." });

            if (buyRequest.Status != BuyRequestStatus.Pending)
                return Json(new { success = false, message = "لا يمكن إلغاء هذا الطلب (البائع سبق وردّ عليه)." });

            var buyer = await _context.Users.FindAsync(buyerId);
            if (buyer == null)
                return Json(new { success = false, message = "المستخدم غير موجود." });

            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                buyer.PendingBalance   -= buyRequest.Amount;
                buyer.WalletBalance    += buyRequest.Amount;
                buyRequest.Item.Status  = ItemStatus.Available;
                buyRequest.Status       = BuyRequestStatus.Cancelled;
                buyRequest.UpdatedAt    = DateTime.Now;

                _context.Transactions.Add(new Transaction
                {
                    BuyerID       = buyerId!,
                    ItemID        = buyRequest.ItemID,
                    FinalPrice    = buyRequest.Amount,
                    Status        = OrderStatus.Cancelled,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type          = TransactionType.Refund,
                    Notes         = $"إلغاء طلب شراء من المشتري — {buyRequest.Item.Title}",
                    CreatedAt     = DateTime.Now
                });

                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();

                return Json(new { success = true, message = "تم إلغاء الطلب. رُدّ المبلغ لمحفظتك." });
            }
            catch (Exception)
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "حدث خطأ. يرجى المحاولة لاحقاً." });
            }
        }

        // =====================================================================
        //  GET: Transactions/TopUp
        // =====================================================================
        [HttpGet]
        public async Task<IActionResult> TopUp()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);
            ViewBag.CurrentBalance = user?.WalletBalance ?? 0;
            return View();
        }

        // =====================================================================
        //  POST: Transactions/TopUpWallet
        // =====================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopUpWallet(decimal amount)
        {
            if (amount <= 0 || amount > 10000)
                return Json(new { success = false, message = "المبلغ يجب أن يكون بين $1 و$10,000." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return Json(new { success = false, message = "المستخدم غير موجود." });

            await using var dbTx = await _context.Database.BeginTransactionAsync();
            try
            {
                user.WalletBalance += amount;

                _context.Transactions.Add(new Transaction
                {
                    BuyerID       = userId!,
                    FinalPrice    = amount,
                    Status        = OrderStatus.Completed,
                    PaymentMethod = PaymentMethod.Wallet,
                    Type          = TransactionType.Deposit,
                    Notes         = $"شحن رصيد بمبلغ ${amount:0.00}",
                    CreatedAt     = DateTime.Now
                });

                await _context.SaveChangesAsync();
                await dbTx.CommitAsync();

                return Json(new { success = true, message = $"تم شحن محفظتك بمبلغ ${amount:0.00}! رصيدك الجديد: ${user.WalletBalance:0.00}" });
            }
            catch (Exception)
            {
                await dbTx.RollbackAsync();
                return Json(new { success = false, message = "حدث خطأ أثناء شحن الرصيد." });
            }
        }

        // =====================================================================
        //  GET: Transactions/GetWalletBalance — API
        // =====================================================================
        [HttpGet]
        public async Task<IActionResult> GetWalletBalance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);
            return Json(new { balance = user?.WalletBalance ?? 0, pending = user?.PendingBalance ?? 0 });
        }
    }
}
