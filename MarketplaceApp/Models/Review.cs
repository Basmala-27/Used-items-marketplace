using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{

    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReviewID { get; set; }

        [Required(ErrorMessage = "Transaction reference is required")]
        [Display(Name = "Related Transaction")]
        public int TransactionID { get; set; }


        [ForeignKey(nameof(TransactionID))]
        public virtual Transaction Transaction { get; set; } = null!;

        [Required(ErrorMessage = "Reviewer reference is required")]
        [Display(Name = "Reviewer")]
        public int ReviewerID { get; set; }  // Should it changed to string ?

        [Required(ErrorMessage = "Seller reference is required")]
        [Display(Name = "Seller")]
        public string SellerID { get; set; } = string.Empty;


        [Required(ErrorMessage = "Please provide a rating")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars")]
        [Display(Name = "Rating", Prompt = "Select 1-5 stars")]
        public int Rating { get; set; }


        [StringLength(500, ErrorMessage = "Comment can't exceed 500 characters")]
        [Display(Name = "Review Comment", Prompt = "Share your experience with this seller...")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; } = string.Empty;


        [Required]
        [Display(Name = "Date Posted")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        [ForeignKey(nameof(ReviewerID))]
        [InverseProperty("ReviewsWritten")] // Connects to the Reviewer's list
        public virtual ApplicationUser Reviewer { get; set; } = null!;

        [ForeignKey(nameof(SellerID))]
        [InverseProperty("ReviewsReceived")] // Connects to the Seller's list
        public virtual ApplicationUser Seller { get; set; } = null!;

    }
}