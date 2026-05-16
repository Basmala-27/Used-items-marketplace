using MarketplaceApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace MarketplaceApp.ViewModels
{
    public class ItemEditViewModel
    {
        public int ItemID { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        public string Condition { get; set; } = string.Empty;

        public string? Location { get; set; }

        [Required]
        public ItemStatus Status { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public string UserID { get; set; } = string.Empty;

        public bool IsAvailableForSale { get; set; }
        public bool IsAvailableForSwap { get; set; }

        public string? CategoryName { get; set; }
        public string? UserEmail { get; set; }
    }
}