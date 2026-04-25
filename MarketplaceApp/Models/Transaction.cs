using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

using MarketplaceApp.Enums;

namespace MarketplaceApp.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        public int ItemID { get; set; }

        public int BuyerID { get; set; }

        public int SellerID { get; set; }

        public decimal FinalPrice { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public virtual Item Item { get; set; } = null!;
        public virtual User Buyer { get; set; } = null!;
        public virtual User Seller { get; set; } = null!;

        public virtual Review? Review { get; set; }
    }
}