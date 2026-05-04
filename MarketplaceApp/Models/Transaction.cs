using MarketplaceApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        [Display(Name = "Purchased Item")]
        public int? ItemID { get; set; }

        [Required]
        [Display(Name = "Buyer")]
        public string BuyerID { get; set; } = string.Empty;

        [Display(Name = "Seller")]
        public string? SellerID { get; set; }

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
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Wallet;

        /// <summary>نوع العملية: شراء أو إيداع (شحن رصيد) أو استرداد</summary>
        [Required]
        [Display(Name = "Transaction Type")]
        public TransactionType Type { get; set; } = TransactionType.Purchase;

        [StringLength(500)]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Required]
        [Display(Name = "Transaction Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        // --- Navigation Properties ---

        [ForeignKey(nameof(ItemID))]
        public virtual Item? Item { get; set; }

        [ForeignKey(nameof(BuyerID))]
        [InverseProperty("Purchases")] 
        public virtual ApplicationUser Buyer { get; set; } = null!;

        [ForeignKey(nameof(SellerID))]
        [InverseProperty("Sales")] 
        public virtual ApplicationUser? Seller { get; set; }

        [Display(Name = "Customer Review")]
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}