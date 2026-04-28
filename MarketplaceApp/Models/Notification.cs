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
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public bool IsRead { get; set; } = false;

        [Required]
        public NotificationType Type { get; set; } = NotificationType.System;
        public string? MessageText { get; set; }

        public int? RelatedOfferID { get; set; }

        [ForeignKey("RelatedOfferID")]
        public Offer? Offer { get; set; }

        [Required]
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; } = null!;
    }
}