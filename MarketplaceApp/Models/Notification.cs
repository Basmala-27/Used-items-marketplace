using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MarketplaceApp.Enums;

namespace MarketplaceApp.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        [Required]
        public string UserID { get; set; } = string.Empty;

        [ForeignKey(nameof(UserID))]
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public NotificationType Type { get; set; }

        [Required]
        [StringLength(500)]
        public string MessageText { get; set; } = string.Empty;

        // 🔗 لينك لما المستخدم يضغط على الإشعار
        public string? TargetUrl { get; set; }

        // 🔗 ID مرتبط (Offer / Message / Transaction)
        public int? RelatedEntityID { get; set; }

        public bool IsRead { get; set; } = false;

        // ⏱ خليها UTC عشان تتجنب مشاكل الوقت
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}