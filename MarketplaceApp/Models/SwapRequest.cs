using MarketplaceApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    public class SwapRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SwapRequestId { get; set; }



        [Required(ErrorMessage = "Requester reference is required")]
        [Display(Name = "Sender ID")]
        public string RequesterId { get; set; } = string.Empty;


        [Required(ErrorMessage = "Offered item is required")]
        [Display(Name = "My Item", Prompt = "The item you are offering to swap")]
        public int OfferedItemId { get; set; }


        [Required(ErrorMessage = "Requested item is required")]
        [Display(Name = "Desired Item", Prompt = "The item you want to receive")]
        public int RequestedItemId { get; set; }


        [Required]
        [Display(Name = "Request Status")]
        public OfferStatus Status { get; set; } = OfferStatus.Pending;


        [Required]
        [Display(Name = "Sent On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }= DateTime.Now;


        // --- Navigation Properties ---

        [ForeignKey(nameof(RequesterId))]
        [InverseProperty("SentSwapRequests")]
        public virtual ApplicationUser Requester { get; set; } = null!;

        [ForeignKey(nameof(OfferedItemId))]
        public virtual Item OfferedItem { get; set; } = null!;

        [ForeignKey(nameof(RequestedItemId))]
        public virtual Item RequestedItem { get; set; } = null!;
    }
}