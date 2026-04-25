using System.ComponentModel.DataAnnotations;

namespace MarketplaceApp.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

      
        [MaxLength(100)]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }= string.Empty;



        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [Required(ErrorMessage = "Password is required")]
        public string PasswordHash { get; set; }= string.Empty;

        [Phone]
        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }= string.Empty;

        [MaxLength(200)]
        public string Location { get; set; }= string.Empty;

        [Url]
        public string ProfileImage { get; set; }= string.Empty;

        [Range(0, 5)]
        public double TrustScore { get; set; } = 0;
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<Item> Items { get; set; }= new List<Item>();

        
        // Notification
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        // Favourites
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
