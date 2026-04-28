using MarketplaceApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace MarketplaceApp.Models
{
    public class SwapRequest
    {
        [Key]
        public int SwapRequestId { get; set; }
        [Required]
        public string RequesterId { get; set; }
        [Required]
        public int OfferedItemId { get; set; }
        [Required]
        public int RequestedItemId { get; set; }
        [Required]
        public ApplicationUser Requester { get; set; }
        public OfferStatus Status { get; set; } = OfferStatus.Pending;
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }= DateTime.Now;
    }
}