using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }

        [Required(ErrorMessage = "Conversation reference is required")]
        [Display(Name = "Conversation ID")]
        public int ConversationID { get; set; }

        [Required(ErrorMessage = "Sender reference is required")]
        [Display(Name = "Sender")]
        public string SenderID { get; set; }=string.Empty;



        [Required(ErrorMessage = "Message cannot be empty")]
        [StringLength(1000, ErrorMessage = "Message is too long")]
        [Display(Name = "Message Text", Prompt = "Type your message here...")]
        [RegularExpression(@"^[a-zA-Z0-9].*", ErrorMessage = "Message must start with a letter or number")]
        public string MessageText { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Sent At")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; }

        [ForeignKey(nameof(ConversationID))]
        public virtual Conversation Conversation { get; set; } = null!;

        [ForeignKey(nameof(SenderID))]
        public virtual ApplicationUser Sender { get; set; } = null!;
    }
}
