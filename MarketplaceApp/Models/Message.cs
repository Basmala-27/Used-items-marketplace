using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [Required]
        public int ConversationID { get; set; }

        [Required]
        public int SenderID { get; set; }

        [Required(ErrorMessage = "Message cannot be empty")]
        public string MessageText { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("ConversationID")]
        public virtual Conversation Conversation { get; set; } = null!;

        [ForeignKey("SenderID")] 
        public virtual User Sender { get; set; } = null!;
    }
}