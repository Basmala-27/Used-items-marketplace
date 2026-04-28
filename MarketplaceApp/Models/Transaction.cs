using MarketplaceApp.Enums;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TransactionID { get; set; }

        [Required(ErrorMessage = "Item reference is required")]
        [Display(Name = "Purchased Item")]
        public int ItemID { get; set; }

        [Required]
        [Display(Name = "Buyer")]
        public string BuyerID { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Seller")]
        public string SellerID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Final sale price is required")]
        [Range(0.01, 10000000, ErrorMessage = "Price must be a positive value")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Final Price", Prompt = "0.00")]
        public decimal FinalPrice { get; set; }

        [Required]
        [Display(Name = "Order Status")]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required]
        [Display(Name = "Payment Type")]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

        [Required]
        [Display(Name = "Transaction Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        // --- Navigation Properties ---

        [ForeignKey(nameof(ItemID))]
        public virtual Item Item { get; set; } = null!;

        [ForeignKey(nameof(BuyerID))]
        [InverseProperty("Purchases")] 
        public virtual ApplicationUser Buyer { get; set; } = null!;

        [ForeignKey(nameof(SellerID))]
        [InverseProperty("Sales")] 
        public virtual ApplicationUser Seller { get; set; } = null!;

        public virtual Review? Review { get; set; }


        [Display(Name = "Customer Review")]
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}