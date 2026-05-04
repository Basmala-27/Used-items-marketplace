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

        // ================= DB SETS =================
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
        public DbSet<BuyRequest> BuyRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================= CONVERSATION =================
            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.HasOne(c => c.Buyer)
                    .WithMany(u => u.BoughtConversations)
                    .HasForeignKey(c => c.BuyerID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Seller)
                    .WithMany(u => u.SoldConversations)
                    .HasForeignKey(c => c.SellerID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(c => new { c.BuyerID, c.SellerID, c.ItemID })
                    .IsUnique();
            });

            // ================= MESSAGE =================
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

            // ================= FAVORITE =================
            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new { f.UserID, f.ItemID })
                .IsUnique();

            // ================= SWAP REQUEST =================
            modelBuilder.Entity<SwapRequest>()
                .HasOne(s => s.Requester)
                .WithMany()
                .HasForeignKey(s => s.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= OFFER =================
            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Buyer)
                .WithMany()
                .HasForeignKey(o => o.BuyerID)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= REVIEW =================
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Transaction)
                .WithMany(t => t.Reviews)
                .HasForeignKey(r => r.TransactionID)
                .OnDelete(DeleteBehavior.Cascade);

            // ================= NOTIFICATION =================
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Restrict);


            // ================= TRANSACTION =================
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasOne(t => t.Buyer)
                    .WithMany(u => u.Purchases)
                    .HasForeignKey(t => t.BuyerID)
                    .OnDelete(DeleteBehavior.Restrict); // Prevents circular delete issues

                entity.HasOne(t => t.Seller)
                    .WithMany(u => u.Sales)
                    .HasForeignKey(t => t.SellerID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ================= BUY REQUEST =================
            modelBuilder.Entity<BuyRequest>(entity =>
            {
                entity.HasOne(b => b.Buyer)
                    .WithMany()
                    .HasForeignKey(b => b.BuyerID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Seller)
                    .WithMany()
                    .HasForeignKey(b => b.SellerID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Item)
                    .WithMany()
                    .HasForeignKey(b => b.ItemID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ================= SEED DATA =================
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryID = 1,
                    Name = "Electronics",
                    ImageUrl = "/images/categories/electronics.jpg"
                },
                new Category
                {
                    CategoryID = 2,
                    Name = "Furniture",
                    ImageUrl = "/images/categories/furniture.jpg"
                },
                new Category
                {
                    CategoryID = 3,
                    Name = "Fashion",
                    ImageUrl = "/images/categories/fashion.jpg"
                }
            );
        }
    }
}