using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MarketplaceApp.Enums;

namespace MarketplaceApp.Models
{
    public class Offer
    {
        [Key]
        public int OfferID { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public OfferStatus Status { get; set; } = OfferStatus.Pending;
        [Required]
        public decimal Price { get; set; }

        public string? Location { get; set; }

        public string? Condition { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public int ItemID { get; set; }

        [ForeignKey("ItemID")]
        public Item Item { get; set; } = null!;

        [Required]
        public int BuyerID { get; set; }

        [ForeignKey("BuyerID")]
        public User Buyer { get; set; } = null!;
    }
}