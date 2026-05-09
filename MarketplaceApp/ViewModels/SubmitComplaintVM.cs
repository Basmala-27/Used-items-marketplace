using System.ComponentModel.DataAnnotations;

namespace MarketplaceApp.ViewModels
{
    public class SubmitComplaintVM
    {
        [Required(ErrorMessage = "Please provide a title for your complaint")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        [Display(Name = "Complaint Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide the details of your complaint")]
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        [Display(Name = "Details")]
        public string Description { get; set; } = string.Empty;

        // Hidden fields to link the complaint to an item or a user
        public string? TargetUserId { get; set; }

        public int? TargetItemId { get; set; }
    }
}
