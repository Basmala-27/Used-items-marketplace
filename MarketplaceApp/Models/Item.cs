using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Item
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ItemID { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title can't exceed 200 characters")]
    public string Title { get; set; }

    [StringLength(1000)]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, 100000000, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [StringLength(50)]
    public string Condition { get; set; }

    [StringLength(100)]
    public string Location { get; set; }

    [StringLength(50)]
    public string Status { get; set; }

    // FK
    [Required]
    public int UserID { get; set; }

    [Required]
    public int CategoryID { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation Properties
    [ForeignKey(nameof(UserID))]
    public User User { get; set; }

    [ForeignKey(nameof(CategoryID))]
    public Category Category { get; set; }

    // Relationships
    public ICollection<ItemImage> Images { get; set; } = new List<ItemImage>();
}