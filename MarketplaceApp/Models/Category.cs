using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
        [Display(Name = "Category Name", Prompt = "e.g., Electronics, Furniture...")]
        [RegularExpression(@"^[A-Z][a-zA-Z\s]*$", ErrorMessage = "Category name must start with a capital letter")]
        public string Name { get; set; } = string.Empty;

        // --- Navigation Properties ---

        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}