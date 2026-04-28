using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ItemImage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ImageID { get; set; }

    [Required(ErrorMessage = "Image URL is required")]
    [StringLength(500, ErrorMessage = "Image URL can't exceed 500 characters")]
    [DataType(DataType.ImageUrl)]
    [Display(Name = "Image File Path", Prompt = "Enter the URL or file path of the image")]
    [RegularExpression(@"^(http|https|/).*", ErrorMessage = "URL must start with http, https, or /")]
    public string ImageUrl { get; set; } = string.Empty;

    // FK
    [Required(ErrorMessage = "You must link this image to an item")]
    [Display(Name = "Related Item ID")]
    public int ItemID { get; set; }

    [ForeignKey(nameof(ItemID))]
    public virtual Item Item { get; set; } = null!;
}
