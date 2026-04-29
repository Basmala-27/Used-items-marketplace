using MarketplaceApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Item
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ItemID { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title can't exceed 200 characters")]
    [Display(Name = "Item Title", Prompt = "Enter a catchy title for your item")]
    [RegularExpression(@"^[A-Z].*", ErrorMessage = "The title must start with a capital letter")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    [Required(ErrorMessage = "Description is required")] // 
    [DataType(DataType.MultilineText)]
    [Display(Name = "Item Description", Prompt = "Describe the item's features")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, 100000000, ErrorMessage = "Price must be greater than 0")]
    [Display(Name = "Asking Price", Prompt = "0.00")]
    public decimal Price { get; set; }



    [Required(ErrorMessage = "Condition is required")] //
    [StringLength(50)]
    [Display(Name = "Item Condition", Prompt = "e.g., New, Like New, Used")]
    public string Condition { get; set; } = string.Empty;


    [StringLength(100)]
    [Display(Name = "Pickup Location", Prompt = "City or Neighborhood")]
    public string? Location { get; set; } // nullable



    [Required(ErrorMessage = "Status is required")]
    [StringLength(50)]
    [Display(Name = "Listing Status")]
    public string Status { get; set; } = "Available";  // Default is Available

    // FK
    [Required]
    [Display(Name = "Seller ID")]
    public string UserID { get; set; }= string.Empty;

    [Required]
    [Display(Name = "Category ID")]
    public int CategoryID { get; set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "Date Posted")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation Properties


    [ForeignKey(nameof(UserID))]
    public virtual ApplicationUser User { get; set; } = null!;  // making it virtual to allow lazy loading (data access pattern)

    [ForeignKey(nameof(CategoryID))]
    public virtual Category Category { get; set; } = null!;

    // Relationships
    public virtual ICollection<ItemImage> Images { get; set; } = new List<ItemImage>();

    // Favourites 
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();


    [InverseProperty(nameof(SwapRequest.OfferedItem))]
    [Display(Name = "Swaps where this item is offered")]
    public virtual ICollection<SwapRequest> OfferedInSwaps { get; set; } = new List<SwapRequest>();

    [InverseProperty(nameof(SwapRequest.RequestedItem))]
    [Display(Name = "Swaps where this item is requested")]
    public virtual ICollection<SwapRequest> RequestedInSwaps { get; set; } = new List<SwapRequest>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}