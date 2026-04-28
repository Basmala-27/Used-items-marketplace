using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "You should enter the name")]
        [Display(Name = "Full Name", Prompt = "Enter The Name")]
        [RegularExpression(@"^[A-Z][a-zA-Z\s]*$", ErrorMessage = "The name must start with a capital letter")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email Address", Prompt = "example@mail.com")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone format")]
        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone Number", Prompt = "01xxxxxxxxx")]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(200)]
        [Display(Name = "Current Location", Prompt = "City, Country")]
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Profile Picture")]
        public string ProfileImage { get; set; } = string.Empty;

        [Range(0, 5, ErrorMessage = "Trust Score must be between 0 and 5")]
        public double TrustScore { get; set; } = 0;

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // --- Navigation Properties ---

        public virtual ICollection<Item> Items { get; set; } = new List<Item>();

        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        [InverseProperty("Buyer")]
        public virtual ICollection<Conversation> BoughtConversations { get; set; } = new List<Conversation>();

        [InverseProperty("Seller")]
        public virtual ICollection<Conversation> SoldConversations { get; set; } = new List<Conversation>();

        [Display(Name = "My Offers")]
        public virtual ICollection<Offer> BoughtOffers { get; set; } = new List<Offer>();

        [Display(Name = "Reviews I've Written")]
        public virtual ICollection<Review> ReviewsWritten { get; set; } = new List<Review>();

        [Display(Name = "Reviews I've Received")]
        public virtual ICollection<Review> ReviewsReceived { get; set; } = new List<Review>();

        [Display(Name = "My Sent Swaps")]
        public virtual ICollection<SwapRequest> SentSwapRequests { get; set; } = new List<SwapRequest>();


        [Display(Name = "My Purchases")]
        public virtual ICollection<Transaction> Purchases { get; set; } = new List<Transaction>();

        [Display(Name = "My Sales")]
        public virtual ICollection<Transaction> Sales { get; set; } = new List<Transaction>();
    }
}