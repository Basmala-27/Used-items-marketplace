using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ItemImage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ImageID { get; set; }

    [Required(ErrorMessage = "Image URL is required")]
    [StringLength(500, ErrorMessage = "Image URL can't exceed 500 characters")]
    [DataType(DataType.ImageUrl)]
    public string ImageUrl { get; set; }

    // FK
    [Required]
    public int ItemID { get; set; }

    [ForeignKey(nameof(ItemID))]
    public Item Item { get; set; }
}