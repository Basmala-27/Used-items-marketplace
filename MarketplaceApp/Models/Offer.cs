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
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public OfferStatus Status { get; set; } = OfferStatus.Pending;
        [Required]
        public decimal Price { get; set; }


        [Required]
        public int ItemID { get; set; }

        [ForeignKey("ItemID")]
        public Item Item { get; set; } = null!;

        [Required]
        public string BuyerID { get; set; }

        [ForeignKey("BuyerID")]
        public ApplicationUser Buyer { get; set; } = null!;
    }
}