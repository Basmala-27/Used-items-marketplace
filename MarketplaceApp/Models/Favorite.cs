using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    public class Favorite
    {
        [Key]
        public int FavoriteID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public int ItemID { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey(nameof(UserID))]
        public ApplicationUser User { get; set; } = null!;

        [ForeignKey(nameof(ItemID))]
        public Item Item { get; set; } = null!;
    }
}