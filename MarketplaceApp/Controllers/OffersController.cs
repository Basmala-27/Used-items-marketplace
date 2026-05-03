using MarketplaceApp.Data;
using MarketplaceApp.Enums;
using MarketplaceApp.Models;
using MarketplaceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApp.Controllers
{
    public class OffersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public OffersController(ApplicationDbContext context,
                                INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // ================= INDEX =================
        public async Task<IActionResult> Index()
        {
            var offers = _context.Offers
                .Include(o => o.Buyer)
                .Include(o => o.Item);

            return View(await offers.ToListAsync());
        }

        // ================= DETAILS =================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var offer = await _context.Offers
                .Include(o => o.Buyer)
                .Include(o => o.Item)
                .FirstOrDefaultAsync(m => m.OfferID == id);

            if (offer == null) return NotFound();

            return View(offer);
        }

        // ================= CREATE (GET) =================
        public IActionResult Create()
        {
            ViewData["BuyerID"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Title");
            return View();
        }

        // ================= CREATE (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Offer offer)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns(offer);
                return View(offer);
            }

            offer.CreatedAt = DateTime.UtcNow;
            offer.Status = OfferStatus.Pending;

            _context.Add(offer);
            await _context.SaveChangesAsync();

            // ?? NOTIFICATION ??? ????
            var item = await _context.Items.FindAsync(offer.ItemID);

            if (item != null)
            {
                await _notificationService.SendAsync(
                    item.UserID,                       // userId
                    NotificationType.OfferCreated,     // type
                    offer.OfferID,                     // relatedId (int?)
                    "You received a new offer",        // message
                    $"/Offers/Details/{offer.OfferID}" // url
                );
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= EDIT =================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var offer = await _context.Offers.FindAsync(id);
            if (offer == null) return NotFound();

            LoadDropdowns(offer);
            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Offer offer)
        {
            if (id != offer.OfferID)
                return NotFound();

            if (!ModelState.IsValid)
            {
                LoadDropdowns(offer);
                return View(offer);
            }

            try
            {
                _context.Update(offer);
                await _context.SaveChangesAsync();

                var item = await _context.Items.FindAsync(offer.ItemID);

                if (item != null)
                {
                    if (offer.Status == OfferStatus.Accepted)
                    {
                        await _notificationService.SendAsync(
                            offer.BuyerID,
                            NotificationType.OfferAccepted,
                            offer.OfferID,
                            "Your offer was accepted",
                            $"/Offers/Details/{offer.OfferID}"
                        );
                    }
                    else if (offer.Status == OfferStatus.Rejected)
                    {
                        await _notificationService.SendAsync(
                            offer.BuyerID,
                            NotificationType.OfferRejected,
                            offer.OfferID,
                            "Your offer was rejected",
                            $"/Offers/Details/{offer.OfferID}"
                        );
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfferExists(offer.OfferID))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE =================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var offer = await _context.Offers
                .Include(o => o.Buyer)
                .Include(o => o.Item)
                .FirstOrDefaultAsync(m => m.OfferID == id);

            if (offer == null) return NotFound();

            return View(offer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offer = await _context.Offers.FindAsync(id);

            if (offer != null)
            {
                _context.Offers.Remove(offer);

                await _notificationService.SendAsync(
                    offer.BuyerID,
                    NotificationType.System,
                    offer.OfferID,
                    "Your offer was deleted"
                );
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ================= HELPERS =================
        private void LoadDropdowns(Offer? offer = null)
        {
            ViewData["BuyerID"] = new SelectList(_context.Users, "Id", "Email", offer?.BuyerID);
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Title", offer?.ItemID);
        }

        private bool OfferExists(int id)
        {
            return _context.Offers.Any(e => e.OfferID == id);
        }
    }
}