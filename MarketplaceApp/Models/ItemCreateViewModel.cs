using System.ComponentModel.DataAnnotations;

namespace MarketplaceApp.Models
{
    public class ItemCreateViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "The title must start with a capital letter")]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100000000)]
        public decimal Price { get; set; }

        [Required]
        public string Condition { get; set; } = string.Empty;

        public string? Location { get; set; }

        [Required]
        public int CategoryID { get; set; }

     
        [Display(Name = "Item Photos")]
        public List<IFormFile>? ImageFiles { get; set; }
    }
}
