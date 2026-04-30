using MarketplaceApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MarketplaceApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SwapRequest> SwapRequests { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. إعداد البريد الإلكتروني كحقل فريد
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // 2. إعدادات المحادثات (Conversation) - علاقة البائع والمشتري
            modelBuilder.Entity<Conversation>(entity =>
            {
                // تعريف علاقة المشتري
                entity.HasOne(c => c.Buyer)
                    .WithMany(u => u.BoughtConversations)
                    .HasForeignKey(c => c.BuyerID)
                    .OnDelete(DeleteBehavior.Restrict);

                // تعريف علاقة البائع
                entity.HasOne(c => c.Seller)
                    .WithMany(u => u.SoldConversations)
                    .HasForeignKey(c => c.SellerID)
                    .OnDelete(DeleteBehavior.Restrict);

                // منع تكرار نفس المحادثة لنفس المشتري والبائع على نفس المنتج
                entity.HasIndex(c => new { c.BuyerID, c.SellerID, c.ItemID })
                    .IsUnique();
            });

            // 3. إعدادات الرسائل (Message)
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasOne(m => m.Sender)
                    .WithMany()
                    .HasForeignKey(m => m.SenderID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Conversation)
                    .WithMany(c => c.Messages)
                    .HasForeignKey(m => m.ConversationID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 4. المفتاح المركب للمفضلات
            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new { f.UserID, f.ItemID })
                .IsUnique();

            // 5. علاقة طلبات التبادل
            modelBuilder.Entity<SwapRequest>()
                .HasOne(s => s.Requester)
                .WithMany()
                .HasForeignKey(s => s.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            // 6. علاقة العروض
            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Buyer)
                .WithMany()
                .HasForeignKey(o => o.BuyerID)
                .OnDelete(DeleteBehavior.NoAction);

            // 7. علاقة التقييمات مع المعاملات
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Transaction)
                .WithMany(t => t.Reviews)
                .HasForeignKey(r => r.TransactionID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Category>().HasData(
        new Category
        {
            CategoryID = -1,
            Name = "Electronics",
            ImageUrl = "/images/categories/electronics.jpg"
        },
        new Category
        {
            CategoryID = -2,
            Name = "Furniture",
            ImageUrl = "/images/categories/furniture.jpg"
        },
        new Category
        {
            CategoryID = -3,
            Name = "Fashion",
            ImageUrl = "/images/categories/fashion.jpg"
        }
        );
        }
    }
}