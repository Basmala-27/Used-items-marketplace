using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MarketplaceApp.Enums;

namespace MarketplaceApp.Models
{
    public class Offer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OfferID { get; set; }

        [Required]
        [Display(Name = "Offer Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Offer status is required")]
        [Display(Name = "Current Status")]
        public OfferStatus Status { get; set; } = OfferStatus.Pending;

        [Required(ErrorMessage = "Proposed price is required")]
        [Range(0.01, 10000000, ErrorMessage = "Price must be a positive value")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Offered Price", Prompt = "0.00")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Item reference is required")]
        [Display(Name = "Related Item")]
        public int ItemID { get; set; }

        [Required(ErrorMessage = "Buyer reference is required")]
        [Display(Name = "Buyer ID")]
        public string BuyerID { get; set; } = string.Empty;

        // --- Navigation Properties ---

        [ForeignKey(nameof(ItemID))]
        public virtual Item Item { get; set; } = null!;

        [ForeignKey(nameof(BuyerID))]
        [InverseProperty("BoughtOffers")]
        public virtual ApplicationUser Buyer { get; set; } = null!;

        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}