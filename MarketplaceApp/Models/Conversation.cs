using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MarketplaceApp.Data;

namespace MarketplaceApp.Models
{
    public class Conversation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] //
        public int ConversationID { get; set; }

        [Required(ErrorMessage = "Item reference is required")]
        [Display(Name = "Related Item")]
        [ForeignKey("Item")]
        public int ItemID { get; set; }

        [Required(ErrorMessage = "Buyer reference is required")]
        [ForeignKey("Buyer")]
        [Display(Name = "Buyer Name")]
        public string BuyerID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seller reference is required")]
        [ForeignKey("Seller")]
        [Display(Name = "Seller Name")]
        public string SellerID { get; set; } = string.Empty;

        [Required] // 
        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // --- Navigation Properties ---

        [ForeignKey("ItemID")]
        public virtual Item Item { get; set; } = null!;

        [ForeignKey("BuyerID")]
        [InverseProperty("BoughtConversations")]
        public virtual ApplicationUser Buyer { get; set; } = null!; // Changed to ApplicationUser

        [ForeignKey("SellerID")]
        [InverseProperty("SoldConversations")]
        public virtual ApplicationUser Seller { get; set; } = null!; // Changed to ApplicationUser

        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}