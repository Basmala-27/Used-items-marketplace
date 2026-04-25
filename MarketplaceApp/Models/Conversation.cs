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
        public int BuyerID { get; set; }

        [Required]
        [ForeignKey("Seller")]
        public int SellerID { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Item Item { get; set; }

        public User Buyer { get; set; }=    null!;
        public User Seller { get; set; }= null!;

        public ICollection<Message> Messages { get; set; }= new List<Message>();
    }
}
