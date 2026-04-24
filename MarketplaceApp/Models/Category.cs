using System.ComponentModel.DataAnnotations;

namespace MarketplaceApp.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }

}