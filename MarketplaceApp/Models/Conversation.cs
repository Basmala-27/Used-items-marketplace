using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    public class Conversation
    {
        [Key]
        public int ConversationID { get; set; }

        [Required]
        [ForeignKey("Item")]
        public int ItemID { get; set; }

        [Required]
        [ForeignKey("Buyer")]
        public string BuyerID { get; set; }

        [Required]
        [ForeignKey("Seller")]
        public string SellerID { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Item Item { get; set; }

        public ApplicationUser Buyer { get; set; }=    null!;
        public ApplicationUser Seller { get; set; }= null!;

        public ICollection<Message> Messages { get; set; }= new List<Message>();
    }
}
