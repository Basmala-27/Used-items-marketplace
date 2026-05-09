using MarketplaceApp.Data;
using MarketplaceApp.Models;
using MarketplaceApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MarketplaceApp.Controllers
{
    [Authorize]
    public class ComplaintsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComplaintsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create(string? targetUserId, int? targetItemId)
        {
            var model = new SubmitComplaintVM
            {
                TargetUserId = targetUserId,
                TargetItemId = targetItemId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubmitComplaintVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var complaint = new Complaint
            {
                Title = model.Title,
                Description = model.Description,
                ComplainantId = userId,
                TargetUserId = model.TargetUserId,
                TargetItemId = model.TargetItemId,
                Status = ComplaintStatus.Pending,
                CreatedAt = DateTime.Now,
                IsReadByAdmin = false
            };

            _context.Complaints.Add(complaint);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your complaint has been submitted successfully and will be reviewed by our team.";

            // Redirect based on where they came from
            if (model.TargetItemId.HasValue)
            {
                return RedirectToAction("Details", "Items", new { id = model.TargetItemId });
            }
            if (!string.IsNullOrEmpty(model.TargetUserId))
            {
                return RedirectToAction("SellerProfile", "Profile", new { id = model.TargetUserId });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
