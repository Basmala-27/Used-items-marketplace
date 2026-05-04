using Microsoft.AspNetCore.SignalR;
using MarketplaceApp.Data;
using MarketplaceApp.Enums;
using MarketplaceApp.Models;
using MarketplaceApp.Hubs;

namespace MarketplaceApp.Services
{
    public interface INotificationService
    {
        Task SendAsync(string userId, NotificationType type, int? relatedId, string message, string? url = null);
        Task NotifyBuyRequestAsync(string userId, int buyRequestId, string itemTitle, decimal price);
        Task NotifyBuyRequestAcceptedAsync(string userId, int buyRequestId, string itemTitle);
        Task NotifyBuyRequestRejectedAsync(string userId, int buyRequestId, string itemTitle, decimal refundAmount);
        Task NotifyOrderCompletedAsync(string sellerId, int buyRequestId, string itemTitle, decimal amount);
        Task NotifySwapRequestAsync(string userId, int swapRequestId, string requestedItemTitle);
        Task NotifySwapRequestAcceptedAsync(string userId, int swapRequestId, string requestedItemTitle);
        Task NotifySwapRequestRejectedAsync(string userId, int swapRequestId, string requestedItemTitle);
        Task NotifyNewMessageAsync(string userId, int conversationId, string senderName);
    }

    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task SendAsync(string userId, NotificationType type, int? relatedId, string message, string? url = null)
        {

            var notification = new Notification
            {
                UserID = userId,
                Type = type,
                RelatedEntityID = relatedId,
                MessageText = message,
                TargetUrl = url,
                CreatedAt = DateTime.Now,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", new
            {
                title = type.ToString(),
                message = message,
                url = url,
                createdAt = notification.CreatedAt.ToString("yyyy-MM-dd HH:mm")
            });
        }

        public async Task NotifyBuyRequestAsync(string userId, int buyRequestId, string itemTitle, decimal price)
        {
            await SendAsync(userId, NotificationType.Order, buyRequestId,
                $"🛒 طلب شراء جديد لمنتجك \"{itemTitle}\" بسعر ${price:0.00} — قبل أو ارفض الطلب",
                $"/Transactions/SellerRequests");
        }

        public async Task NotifyBuyRequestAcceptedAsync(string userId, int buyRequestId, string itemTitle)
        {
            await SendAsync(userId, NotificationType.Order, buyRequestId,
                $"✅ البائع قبل طلب شرائك لـ \"{itemTitle}\"! جارٍ الشحن. أكّد استلامك لتحرير المبلغ.",
                $"/Transactions/MyOrders");
        }

        public async Task NotifyBuyRequestRejectedAsync(string userId, int buyRequestId, string itemTitle, decimal refundAmount)
        {
            await SendAsync(userId, NotificationType.Order, buyRequestId,
                $"❌ رفض البائع طلب شرائك لـ \"{itemTitle}\". تم استرداد ${refundAmount:0.00} لمحفظتك.",
                $"/Transactions/MyOrders");
        }

        public async Task NotifyOrderCompletedAsync(string sellerId, int buyRequestId, string itemTitle, decimal amount)
        {
            await SendAsync(sellerId, NotificationType.Transaction, buyRequestId,
                $"✅ أكد المشتري استلام \"{itemTitle}\". تم تحويل ${amount:0.00} (50%) الأخيرة لمحفظتك!",
                $"/Transactions");
        }

        public async Task NotifySwapRequestAsync(string userId, int swapRequestId, string requestedItemTitle)
        {
            await SendAsync(userId, NotificationType.SwapRequest, swapRequestId,
                $"🔄 لديك طلب تبادل جديد على منتجك \"{requestedItemTitle}\".",
                $"/Transactions/SellerRequests");
        }

        public async Task NotifySwapRequestAcceptedAsync(string userId, int swapRequestId, string requestedItemTitle)
        {
            await SendAsync(userId, NotificationType.SwapRequest, swapRequestId,
                $"✅ تمت الموافقة على طلب التبادل الخاص بك لمنتج \"{requestedItemTitle}\".",
                $"/SwapRequests/Details/{swapRequestId}");
        }

        public async Task NotifySwapRequestRejectedAsync(string userId, int swapRequestId, string requestedItemTitle)
        {
            await SendAsync(userId, NotificationType.SwapRequest, swapRequestId,
                $"❌ تم رفض طلب التبادل الخاص بك لمنتج \"{requestedItemTitle}\".",
                $"/SwapRequests/Details/{swapRequestId}");
        }

        public async Task NotifyNewMessageAsync(string userId, int conversationId, string senderName)
        {
            await SendAsync(userId, NotificationType.Message, conversationId,
                $"💬 رسالة جديدة من {senderName}.",
                $"/Chat/Chat/{conversationId}");
        }
    }
}