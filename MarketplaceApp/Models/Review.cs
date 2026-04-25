using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
   
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }
        public int TransactionID { get; set; }
        [ForeignKey("TransactionID")]
        public Transaction Transaction { get; set; }

        public int ReviewerID { get; set; }
        public int SellerID { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        [StringLength(500, ErrorMessage = "Comment can't exceed 500 characters")]
        public string Comment { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}