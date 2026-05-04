using MarketplaceApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    /// <summary>
    /// يتتبع دورة حياة طلب الشراء من إرساله حتى اكتماله أو رفضه.
    /// منفصل عن Transaction الذي يسجّل حركات المال الفعلية.
    /// </summary>
    public class BuyRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BuyRequestId { get; set; }

        [Required]
        [Display(Name = "Buyer")]
        public string BuyerID { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Seller")]
        public string SellerID { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Item")]
        public int ItemID { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Status")]
        public BuyRequestStatus Status { get; set; } = BuyRequestStatus.Pending;

        /// <summary>ID الـ Transaction الأصلي (حجز الـ Escrow الكامل)</summary>
        public int? EscrowTransactionId { get; set; }

        [Required]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // --- Navigation Properties ---

        [ForeignKey(nameof(BuyerID))]
        public virtual ApplicationUser Buyer { get; set; } = null!;

        [ForeignKey(nameof(SellerID))]
        public virtual ApplicationUser Seller { get; set; } = null!;

        [ForeignKey(nameof(ItemID))]
        public virtual Item Item { get; set; } = null!;
    }
}
