using System.ComponentModel.DataAnnotations;

namespace MarketplaceApp.Models
{
    public class CreateSwapViewModel
    {
        public int RequestedItemId { get; set; }
        public string RequestedItemName { get; set; }
        public decimal RequestedItemPrice { get; set; }

        [Required(ErrorMessage = "Please select an item to swap")]
        public int OfferedItemId { get; set; }

        public List<Item>? MyAvailableItems { get; set; }
    }
}