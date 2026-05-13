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
        Task NotifyBuyRequestAcceptedAsync(string userId, int buyRequestId, string itemTitle, string? contactNumber = null);
        Task NotifyBuyRequestRejectedAsync(string userId, int buyRequestId, string itemTitle, decimal refundAmount);
        Task NotifyOrderCompletedAsync(string sellerId, int buyRequestId, string itemTitle, decimal amount);
        Task NotifySwapRequestAsync(string userId, int swapRequestId, string requestedItemTitle);
        Task NotifySwapRequestAcceptedAsync(string userId, int swapRequestId, string requestedItemTitle, string? contactNumber = null);
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
                $"🛒 New buy request for your item \"{itemTitle}\" at ${price:0.00} — Accept or reject the request",
                $"/Transactions/SellerRequests");
        }

        public async Task NotifyBuyRequestAcceptedAsync(string userId, int buyRequestId, string itemTitle, string? contactNumber = null)
        {
            string phoneText = !string.IsNullOrEmpty(contactNumber) ? $" Contact: {contactNumber}." : "";
            await SendAsync(userId, NotificationType.Order, buyRequestId,
                $"✅ The seller accepted your buy request for \"{itemTitle}\"! Shipping in progress.{phoneText} Confirm receipt to release payment.",
                $"/Transactions/MyOrders");
        }

        public async Task NotifyBuyRequestRejectedAsync(string userId, int buyRequestId, string itemTitle, decimal refundAmount)
        {
            await SendAsync(userId, NotificationType.Order, buyRequestId,
                $"❌ The seller rejected your buy request for \"{itemTitle}\". ${refundAmount:0.00} has been refunded to your wallet.",
                $"/Transactions/MyOrders");
        }

        public async Task NotifyOrderCompletedAsync(string sellerId, int buyRequestId, string itemTitle, decimal amount)
        {
            await SendAsync(sellerId, NotificationType.Transaction, buyRequestId,
                $"✅ The buyer confirmed receiving \"{itemTitle}\". The final ${amount:0.00} (50%) has been transferred to your wallet!",
                $"/Profile#transactions");
        }

        public async Task NotifySwapRequestAsync(string userId, int swapRequestId, string requestedItemTitle)
        {
            await SendAsync(userId, NotificationType.SwapRequest, swapRequestId,
                $"🔄 You have a new swap request for your item \"{requestedItemTitle}\".",
                $"/Transactions/SellerRequests");
        }

        public async Task NotifySwapRequestAcceptedAsync(string userId, int swapRequestId, string requestedItemTitle, string? contactNumber = null)
        {
            string phoneText = !string.IsNullOrEmpty(contactNumber) ? $" Contact them at: {contactNumber}." : "";
            await SendAsync(userId, NotificationType.SwapRequest, swapRequestId,
                $"✅ Your swap request for \"{requestedItemTitle}\" has been approved.{phoneText}",
                $"/Profile#transactions");
        }

        public async Task NotifySwapRequestRejectedAsync(string userId, int swapRequestId, string requestedItemTitle)
        {
            await SendAsync(userId, NotificationType.SwapRequest, swapRequestId,
                $"❌ Your swap request for \"{requestedItemTitle}\" has been rejected.",
                $"/SwapRequests/MySentRequests");
        }

        public async Task NotifyNewMessageAsync(string userId, int conversationId, string senderName)
        {
            await SendAsync(userId, NotificationType.Message, conversationId,
                $"💬 New message from {senderName}.",
                $"/Chat/Chat/{conversationId}");
        }
    }
}