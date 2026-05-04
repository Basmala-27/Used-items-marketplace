using MarketplaceApp.Data;
using MarketplaceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketplaceApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Services.INotificationService _notificationService;

        public ChatController(ApplicationDbContext context, Services.INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // 🔹 كل الشاتات
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var conversations = _context.Conversations
                .Where(c => c.BuyerID == userId || c.SellerID == userId)
                .Include(c => c.Item)
                .Include(c => c.Buyer)
                .Include(c => c.Seller)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            return View(conversations);
        }

        // 🔹 فتح شات
        public IActionResult Chat(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var conversation = _context.Conversations
                .Include(c => c.Seller)
                .Include(c => c.Buyer)
                .Include(c => c.Item)
                .FirstOrDefault(c => c.ConversationID == id);

            if (conversation == null)
                return NotFound();

            if (conversation.BuyerID != userId && conversation.SellerID != userId)
                return Unauthorized();

            var messages = _context.Messages
                .Where(m => m.ConversationID    == id)
                .OrderBy(m => m.CreatedAt)
                .ToList();

            ViewBag.Conversation = conversation;

            return View(messages);
        }
        // 🔹 إرسال رسالة
        [HttpPost]
        public async Task<IActionResult> SendMessage(int conversationId, string content)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var message = new Message
            {
                ConversationID = conversationId,
                SenderID = userId,
                MessageText = content,
                CreatedAt = DateTime.Now,
                IsRead = false

            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            var conversation = await _context.Conversations.FindAsync(conversationId);
            if (conversation != null)
            {
                var otherUserId = conversation.BuyerID == userId ? conversation.SellerID : conversation.BuyerID;
                var senderUser = await _context.Users.FindAsync(userId);
                string senderName = senderUser?.Name ?? "User";
                await _notificationService.NotifyNewMessageAsync(otherUserId, conversationId, senderName);
            }

            return RedirectToAction("Chat", new { id = conversationId });
        }

        public IActionResult StartConversation(int itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 🔹 نجيب الـ item
            var item = _context.Items.Find(itemId);

            if (item == null)
                return NotFound();

            var sellerId = item.UserID;

            // ❌ تمنعي user يكلم نفسه
            if (sellerId == userId)
                return BadRequest("You can't chat with yourself");

            // 🔍 هل في conversation موجودة؟
            var conversation = _context.Conversations
    .FirstOrDefault(c =>
        c.BuyerID == userId &&
        c.SellerID == sellerId &&
        c.ItemID == itemId
    );

            // 🆕 لو مش موجودة نعمل واحدة
            if (conversation == null)
            {
                conversation = new Conversation
                {
                    BuyerID = userId,
                    SellerID = sellerId,
                    ItemID= itemId,
                    CreatedAt = DateTime.Now
                };

                _context.Conversations.Add(conversation);
                _context.SaveChanges();
            }

            // 🔁 نروح للشات
            return RedirectToAction("Chat", new { id = conversation.ConversationID });
        }
    }
}