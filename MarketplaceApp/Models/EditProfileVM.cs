using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace MarketplaceApp.Models
{
    public class EditProfileVM
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Please enter a valid Egyptian phone number.")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }


        [Display(Name = "Profile Picture")]
        public IFormFile? ProfileImage { get; set; }


        // Used for display only, not for binding from the form
        public string? ExistingImageUrl { get; set; }
    }
}