using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApp.Models
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title can't exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, ErrorMessage = "Description can't exceed 2000 characters")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ComplainantId { get; set; } = string.Empty;

        public string? TargetUserId { get; set; }

        public int? TargetItemId { get; set; }

        public ComplaintStatus Status { get; set; } = ComplaintStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsReadByAdmin { get; set; } = false;

        // Navigation Properties
        [ForeignKey(nameof(ComplainantId))]
        public virtual ApplicationUser Complainant { get; set; } = null!;

        [ForeignKey(nameof(TargetUserId))]
        public virtual ApplicationUser? TargetUser { get; set; }

        [ForeignKey(nameof(TargetItemId))]
        public virtual Item? TargetItem { get; set; }
    }
}
