using MarketplaceApp.Data;
using MarketplaceApp.Models;
using MarketplaceApp.ViewModels;
using MarketplaceApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MarketplaceApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminComplaintsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;

        public AdminComplaintsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index(ComplaintStatus? status)
        {
            var query = _context.Complaints
                .Include(c => c.Complainant)
                .Include(c => c.TargetUser)
                .Include(c => c.TargetItem)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            var complaints = await query
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new AdminComplaintListVM
                {
                    Id = c.Id,
                    Title = c.Title,
                    ComplainantName = c.Complainant.Name,
                    TargetUserName = c.TargetUser != null ? c.TargetUser.Name : null,
                    TargetItemTitle = c.TargetItem != null ? c.TargetItem.Title : null,
                    Status = c.Status,
                    CreatedAt = c.CreatedAt,
                    IsReadByAdmin = c.IsReadByAdmin
                })
                .ToListAsync();

            ViewBag.CurrentStatus = status;
            return View(complaints);
        }

        public async Task<IActionResult> Details(int id)
        {
            var complaint = await _context.Complaints
                .Include(c => c.Complainant)
                .Include(c => c.TargetUser)
                .Include(c => c.TargetItem)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (complaint == null)
            {
                return NotFound();
            }

            if (!complaint.IsReadByAdmin)
            {
                complaint.IsReadByAdmin = true;
                await _context.SaveChangesAsync();
            }

            var model = new AdminComplaintDetailsVM
            {
                Complaint = complaint,
                Complainant = complaint.Complainant,
                TargetUser = complaint.TargetUser,
                TargetItem = complaint.TargetItem
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, ComplaintStatus status)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null) return NotFound();

            complaint.Status = status;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Complaint status updated to {status}.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dismiss(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null) return NotFound();

            complaint.Status = ComplaintStatus.Dismissed;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Complaint has been dismissed.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WarnUser(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null || string.IsNullOrEmpty(complaint.TargetUserId)) return NotFound();

            complaint.Status = ComplaintStatus.Resolved;
            await _context.SaveChangesAsync();

            string message = "You have received a warning from the administration regarding a recent complaint.";
            await _notificationService.SendAsync(complaint.TargetUserId, MarketplaceApp.Enums.NotificationType.System, null, message);

            TempData["SuccessMessage"] = "User has been warned and complaint marked as resolved.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BanUser(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null || string.IsNullOrEmpty(complaint.TargetUserId)) return NotFound();

            var user = await _userManager.FindByIdAsync(complaint.TargetUserId);
            if (user != null)
            {
                user.IsBlocked = true;
                await _userManager.UpdateAsync(user);

                complaint.Status = ComplaintStatus.Resolved;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "User has been banned successfully.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var complaint = await _context.Complaints
                .Include(c => c.TargetItem)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (complaint == null || complaint.TargetItemId == null) return NotFound();

            var item = await _context.Items
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.ItemID == complaint.TargetItemId);

            if (item != null)
            {
                try
                {
                    var favorites = _context.Favorites.Where(f => f.ItemID == item.ItemID);
                    _context.Favorites.RemoveRange(favorites);

                    var conversations = _context.Conversations.Where(c => c.ItemID == item.ItemID);
                    _context.Conversations.RemoveRange(conversations);

                    var buyRequests = _context.BuyRequests.Where(b => b.ItemID == item.ItemID);
                    _context.BuyRequests.RemoveRange(buyRequests);

                    var otherComplaints = _context.Complaints.Where(c => c.TargetItemId == item.ItemID && c.Id != id);
                    foreach (var oc in otherComplaints)
                    {
                        oc.TargetItemId = null;
                    }


                    _context.Items.Remove(item);

                    complaint.Status = ComplaintStatus.Resolved;
                    complaint.TargetItemId = null;

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Item and dependencies cleared.";
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Could not delete item. It may be part of an active transaction.";
                    return RedirectToAction(nameof(Details), new { id });
                }
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}