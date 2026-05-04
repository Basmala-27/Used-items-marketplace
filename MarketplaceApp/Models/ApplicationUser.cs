using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    public class ApplicationUser : IdentityUser
    {
     
        [MaxLength(100)]
        [Required(ErrorMessage = "You should enter the name")]
        [Display(Name = "Full Name")]
        [RegularExpression(@"^[A-Z][a-zA-Z\s]*$", ErrorMessage = "The name must start with a capital letter")]
        public string Name { get; set; } = string.Empty;

      
        [MaxLength(200)]
        [Display(Name = "Current Location")]
        public string Location { get; set; } = string.Empty;

        
        [Display(Name = "Profile Picture")]
        public string ProfileImage { get; set; } = string.Empty;

       
        [Range(0, 5)]
        public double TrustScore { get; set; } = 0;

       
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ===== محفظة الـ Escrow =====

        /// <summary>الرصيد الفعلي للمستخدم (متاح للإنفاق)</summary>
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Wallet Balance")]
        public decimal WalletBalance { get; set; } = 0;

        /// <summary>المبلغ المحتجز في الـ Escrow (صفقات قيد التسليم)</summary>
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Pending Balance")]
        public decimal PendingBalance { get; set; } = 0;


        public virtual ICollection<Item> Items { get; set; } = new List<Item>();

        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        public virtual ICollection<Conversation> BoughtConversations { get; set; } = new List<Conversation>();

        public virtual ICollection<Conversation> SoldConversations { get; set; } = new List<Conversation>();

        public virtual ICollection<Offer> BoughtOffers { get; set; } = new List<Offer>();

        public virtual ICollection<Review> ReviewsWritten { get; set; } = new List<Review>();

        public virtual ICollection<Review> ReviewsReceived { get; set; } = new List<Review>();

        public virtual ICollection<SwapRequest> SentSwapRequests { get; set; } = new List<SwapRequest>();

        public virtual ICollection<Transaction> Purchases { get; set; } = new List<Transaction>();

        public virtual ICollection<Transaction> Sales { get; set; } = new List<Transaction>();
    }
}