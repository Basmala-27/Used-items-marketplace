using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketplaceApp.Data;
using MarketplaceApp.Models;

namespace MarketplaceApp.Controllers
{
    public class ConversationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConversationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Conversations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Conversations.Include(c => c.Buyer).Include(c => c.Item).Include(c => c.Seller);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Conversations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversation = await _context.Conversations
                .Include(c => c.Buyer)
                .Include(c => c.Item)
                .Include(c => c.Seller)
                .FirstOrDefaultAsync(m => m.ConversationID == id);
            if (conversation == null)
            {
                return NotFound();
            }

            return View(conversation);
        }

        // GET: Conversations/Create
        public IActionResult Create()
        {
            ViewData["BuyerID"] = new SelectList(_context.Users, "UserID", "Email");
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Title");
            ViewData["SellerID"] = new SelectList(_context.Users, "UserID", "Email");
            return View();
        }

        // POST: Conversations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConversationID,ItemID,BuyerID,SellerID,CreatedAt")] Conversation conversation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conversation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuyerID"] = new SelectList(_context.Users, "UserID", "Email", conversation.BuyerID);
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Title", conversation.ItemID);
            ViewData["SellerID"] = new SelectList(_context.Users, "UserID", "Email", conversation.SellerID);
            return View(conversation);
        }

        // GET: Conversations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversation = await _context.Conversations.FindAsync(id);
            if (conversation == null)
            {
                return NotFound();
            }
            ViewData["BuyerID"] = new SelectList(_context.Users, "UserID", "Email", conversation.BuyerID);
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Title", conversation.ItemID);
            ViewData["SellerID"] = new SelectList(_context.Users, "UserID", "Email", conversation.SellerID);
            return View(conversation);
        }

        // POST: Conversations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConversationID,ItemID,BuyerID,SellerID,CreatedAt")] Conversation conversation)
        {
            if (id != conversation.ConversationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conversation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConversationExists(conversation.ConversationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuyerID"] = new SelectList(_context.Users, "UserID", "Email", conversation.BuyerID);
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Title", conversation.ItemID);
            ViewData["SellerID"] = new SelectList(_context.Users, "UserID", "Email", conversation.SellerID);
            return View(conversation);
        }

        // GET: Conversations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conversation = await _context.Conversations
                .Include(c => c.Buyer)
                .Include(c => c.Item)
                .Include(c => c.Seller)
                .FirstOrDefaultAsync(m => m.ConversationID == id);
            if (conversation == null)
            {
                return NotFound();
            }

            return View(conversation);
        }

        // POST: Conversations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conversation = await _context.Conversations.FindAsync(id);
            if (conversation != null)
            {
                _context.Conversations.Remove(conversation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private bool ConversationExists(int id)
        {
            return _context.Conversations.Any(e => e.ConversationID == id);
        }
    }
}
