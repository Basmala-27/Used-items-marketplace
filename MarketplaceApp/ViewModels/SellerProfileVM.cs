using MarketplaceApp.Models;
using System.Collections.Generic;

namespace MarketplaceApp.ViewModels
{
    public class SellerProfileVM
    {
        public ApplicationUser Seller { get; set; } = null!;
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<Item> AvailableItems { get; set; } = new List<Item>();
        public List<Review> Reviews { get; set; } = new List<Review>();

        // If the current user has a completed transaction with this seller that they haven't reviewed yet
        public int? EligibleTransactionIdToReview { get; set; }
    }
}
