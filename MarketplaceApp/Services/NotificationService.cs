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
    }
}