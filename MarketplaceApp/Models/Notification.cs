using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsRead { get; set; } = false;

        [Required]
        public string Type { get; set; } = "system";

        public string? MessageText { get; set; }

        public int? RelatedOfferID { get; set; }

        [ForeignKey("RelatedOfferID")]
        public Offer? Offer { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; } = null!;
    }
}