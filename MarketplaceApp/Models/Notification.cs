using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MarketplaceApp.Enums;

namespace MarketplaceApp.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NotificationID { get; set; }

        [Required]
        [Display(Name = "Time Received")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Read Status")]
        public bool IsRead { get; set; } = false;

        [Required(ErrorMessage = "Notification type must be specified")]
        [Display(Name = "Notification Category")]
        public NotificationType Type { get; set; } = NotificationType.System;

        [Required(ErrorMessage = "Notification message is required")]
        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters")]
        [Display(Name = "Content", Prompt = "Notification summary...")]
        public string MessageText { get; set; } = string.Empty;

        [Display(Name = "Related Offer")]
        public int? RelatedOfferID { get; set; }

        [ForeignKey("RelatedOfferID")]
        public virtual Offer? Offer { get; set; }

        [Required(ErrorMessage = "A target user is required")]
        [Display(Name = "Recipient")]
        public string UserID { get; set; }= string.Empty;

        [ForeignKey(nameof(UserID))]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}