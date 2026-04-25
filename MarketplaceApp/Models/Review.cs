namespace MarketplaceApp.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ReviewerId { get; set; }
        public int SellerId { get; set; }
        public int TransactionId { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}