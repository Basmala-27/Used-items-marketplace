using MarketplaceApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace MarketplaceApp.ViewModels
{
    public class ItemEditViewModel
    {
        public int ItemID { get; set; }

        // ===== BASIC INFO =====
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Condition { get; set; } = string.Empty;

        public string? Location { get; set; }

        // ===== STATUS =====
        [Required]
        public ItemStatus Status { get; set; }

        // ===== CATEGORY =====
        [Required]
        public int CategoryID { get; set; }

        // ===== USER (hidden, عشان مايكسرش UI القديم) =====
        public string UserID { get; set; } = string.Empty;

        // ===== 🔥 مهم عشان UI يرجع زي الأول =====
        public bool IsAvailableForSale { get; set; }
        public bool IsAvailableForSwap { get; set; }

        // ===== optional لو كنتِ بتعرضيه في الصفحة =====
        public string? CategoryName { get; set; }
        public string? UserEmail { get; set; }
    }
}