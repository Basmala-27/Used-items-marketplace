using MarketplaceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // الجداول (DbSets)
        public DbSet<User> Users { get; set; }
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
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // 2. إعداد المفتاح المركب للمفضلات لضمان عدم تكرار نفس المنتج لنفس المستخدم
            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new { f.UserID, f.ItemID })
                .IsUnique();

            // 3. علاقة One-to-One بين العملية والتقييم
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Review)
                .WithOne(r => r.Transaction)
                .HasForeignKey<Review>(r => r.TransactionID)
                .OnDelete(DeleteBehavior.Cascade);

            // 4. معالجة علاقات المحادثات (بائع ومشتري) - ضروري جداً لتجنب Multiple Cascade Paths
            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.HasOne(c => c.Buyer)
                    .WithMany()
                    .HasForeignKey(c => c.BuyerID)
                    .OnDelete(DeleteBehavior.Restrict); // نستخدم Restrict لمنع الحذف المتسلسل المتعارض

                entity.HasOne(c => c.Seller)
                    .WithMany()
                    .HasForeignKey(c => c.SellerID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // 5. علاقة طلبات التبادل
            modelBuilder.Entity<SwapRequest>(entity =>
            {
                entity.HasOne(s => s.Requester)
                    .WithMany()
                    .HasForeignKey(s => s.RequesterId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // 6. علاقة العروض مع المستخدمين (إذا كانت موجودة في الموديل)
            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Buyer)
                .WithMany()
                .HasForeignKey(o => o.BuyerID)
                .OnDelete(DeleteBehavior.NoAction);
          
        }
    }
}