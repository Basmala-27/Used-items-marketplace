using MarketplaceApp.Models;

namespace MarketplaceApp.ViewModels
{
    public class AdminComplaintDetailsVM
    {
        public Complaint Complaint { get; set; } = null!;
        public ApplicationUser Complainant { get; set; } = null!;
        public ApplicationUser? TargetUser { get; set; }
        public Item? TargetItem { get; set; }
    }
}
