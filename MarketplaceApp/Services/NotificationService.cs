using MarketplaceApp.Data;
using MarketplaceApp.Enums;
using MarketplaceApp.Models;
using MarketplaceApp.Services.MarketplaceApp.Services;

namespace MarketplaceApp.Services
{
    namespace MarketplaceApp.Services
    {
        public interface INotificationService
        {
            Task SendAsync(string userId, NotificationType type, int? relatedId, string message, string? url = null);
        }
    }

    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendAsync(string userId, NotificationType type, int? relatedId, string message, string? url = null)
        {
            var notification = new Notification
            {
                UserID = userId,
                Type = type,
                RelatedEntityID = relatedId, // الربط بالـ ID اللي بعتيه
                MessageText = message,
                TargetUrl = url,
                CreatedAt = DateTime.Now,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}