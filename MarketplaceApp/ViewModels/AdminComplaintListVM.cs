using MarketplaceApp.Enums;
using System;

namespace MarketplaceApp.ViewModels
{
    public class AdminComplaintListVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ComplainantName { get; set; } = string.Empty;
        public string? TargetUserName { get; set; }
        public string? TargetItemTitle { get; set; }
        public ComplaintStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsReadByAdmin { get; set; }
    }
}
