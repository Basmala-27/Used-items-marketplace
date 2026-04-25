namespace MarketplaceApp.Models
{
    public class SwapRequest
    {
        public int SwapRequestId { get; set; }

        public int RequesterId { get; set; }
        public int OfferedItemId { get; set; }
        public int RequestedItemId { get; set; }

        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}