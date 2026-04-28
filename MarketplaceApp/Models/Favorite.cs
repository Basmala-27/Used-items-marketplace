using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    public class Favorite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FavoriteID { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Item ID is required")]
        [Display(Name = "Favorited Item")]
        public int ItemID { get; set; }

        [Required]
        [Display(Name = "Date Added")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties


        [ForeignKey(nameof(UserID))]
        [Display(Name = "Buyer")]
        public virtual ApplicationUser User { get; set; } = null!;

        [ForeignKey(nameof(ItemID))]
        [Display(Name = "Item")]
        public virtual Item Item { get; set; } = null!;
    }
}