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

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SwapRequest> SwapRequests { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<BuyRequest> BuyRequests { get; set; }
        public DbSet<Complaint> Complaints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                .HasMany(i => i.Images)
                .WithOne(img => img.Item)
                .HasForeignKey(img => img.ItemID)
                .OnDelete(DeleteBehavior.Cascade);

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

                entity.HasOne(c => c.Item)
                    .WithMany()
                    .HasForeignKey(c => c.ItemID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(c => new { c.BuyerID, c.SellerID, c.ItemID })
                    .IsUnique();
            });

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

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasIndex(f => new { f.UserID, f.ItemID })
                    .IsUnique();

                entity.HasOne(f => f.Item)
                    .WithMany()
                    .HasForeignKey(f => f.ItemID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SwapRequest>()
                .HasOne(s => s.Requester)
                .WithMany()
                .HasForeignKey(s => s.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Review>()
                .HasOne(r => r.Transaction)
                .WithMany(t => t.Reviews)
                .HasForeignKey(r => r.TransactionID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasOne(t => t.Buyer)
                    .WithMany(u => u.Purchases)
                    .HasForeignKey(t => t.BuyerID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Seller)
                    .WithMany(u => u.Sales)
                    .HasForeignKey(t => t.SellerID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

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

            modelBuilder.Entity<Complaint>(entity =>
            {
                entity.HasOne(c => c.Complainant)
                    .WithMany(u => u.ComplaintsSubmitted)
                    .HasForeignKey(c => c.ComplainantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.TargetUser)
                    .WithMany(u => u.ComplaintsReceived)
                    .HasForeignKey(c => c.TargetUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.TargetItem)
                    .WithMany(i => i.Complaints)
                    .HasForeignKey(c => c.TargetItemId)
                    .OnDelete(DeleteBehavior.SetNull); // Safe for Item deletion
            });

           modelBuilder.Entity<Category>().HasData(
    new Category { CategoryID = 1, Name = "Electronics", ImageUrl = "/images/categories/electronics.jpg" },
    new Category { CategoryID = 2, Name = "Furniture", ImageUrl = "/images/categories/furniture.jpg" },
    new Category { CategoryID = 3, Name = "Clothes", ImageUrl = "/images/categories/fashion.jpg" },

    new Category { CategoryID = 4, Name = "Phones", ImageUrl = "/images/categories/phones.jpg" },
    new Category { CategoryID = 5, Name = "Laptops", ImageUrl = "/images/categories/laptops.jpg" },
    new Category { CategoryID = 6, Name = "Accessories", ImageUrl = "/images/categories/accessories.jpg" },
    new Category { CategoryID = 7, Name = "Gaming", ImageUrl = "/images/categories/gaming.jpg" },
    new Category { CategoryID = 8, Name = "Vehicles", ImageUrl = "/images/categories/vehicles.jpg" },
    new Category { CategoryID = 9, Name = "Study Materials", ImageUrl = "/images/categories/study.jpg" },
    new Category { CategoryID = 10, Name = "Others", ImageUrl = "/images/categories/others.jpg" }
);
        }
    }
}