using AnnouncementBoard.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnouncementBoard.Data
{
    public class AnnouncementBoardDbContext : DbContext
    {
        public AnnouncementBoardDbContext(DbContextOptions<AnnouncementBoardDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Announcement> Announcements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Category entity configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // SubCategory entity configuration
            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Announcement entity configuration
            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.Status).IsRequired();
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Announcements)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Announcements)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.SubCategory)
                    .WithMany(sc => sc.Announcements)
                    .HasForeignKey(e => e.SubCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            // Seed Categories
            var categories = new[]
            {
                new Category { Id = 1, Name = "Побутова техніка", Description = "Техніка для дому", CreatedAt = seedDate },
                new Category { Id = 2, Name = "Комп'ютерна техніка", Description = "Комп'ютери та комплектуючі", CreatedAt = seedDate },
                new Category { Id = 3, Name = "Смартфони", Description = "Мобільні телефони", CreatedAt = seedDate },
                new Category { Id = 4, Name = "Інше", Description = "Різне", CreatedAt = seedDate }
            };
            modelBuilder.Entity<Category>().HasData(categories);

            // Seed SubCategories
            var subCategories = new[]
            {
                // Побутова техніка
                new SubCategory { Id = 1, Name = "Холодильники", CategoryId = 1, CreatedAt = seedDate },
                new SubCategory { Id = 2, Name = "Пральні машини", CategoryId = 1, CreatedAt = seedDate },
                new SubCategory { Id = 3, Name = "Бойлери", CategoryId = 1, CreatedAt = seedDate },
                new SubCategory { Id = 4, Name = "Печі", CategoryId = 1, CreatedAt = seedDate },
                new SubCategory { Id = 5, Name = "Витяжки", CategoryId = 1, CreatedAt = seedDate },
                new SubCategory { Id = 6, Name = "Мікрохвильові печі", CategoryId = 1, CreatedAt = seedDate },
                
                // Комп'ютерна техніка
                new SubCategory { Id = 7, Name = "ПК", CategoryId = 2, CreatedAt = seedDate },
                new SubCategory { Id = 8, Name = "Ноутбуки", CategoryId = 2, CreatedAt = seedDate },
                new SubCategory { Id = 9, Name = "Монітори", CategoryId = 2, CreatedAt = seedDate },
                new SubCategory { Id = 10, Name = "Принтери", CategoryId = 2, CreatedAt = seedDate },
                new SubCategory { Id = 11, Name = "Сканери", CategoryId = 2, CreatedAt = seedDate },
                
                // Смартфони
                new SubCategory { Id = 12, Name = "Android смартфони", CategoryId = 3, CreatedAt = seedDate },
                new SubCategory { Id = 13, Name = "iOS/Apple смартфони", CategoryId = 3, CreatedAt = seedDate },
                
                // Інше
                new SubCategory { Id = 14, Name = "Одяг", CategoryId = 4, CreatedAt = seedDate },
                new SubCategory { Id = 15, Name = "Взуття", CategoryId = 4, CreatedAt = seedDate },
                new SubCategory { Id = 16, Name = "Аксесуари", CategoryId = 4, CreatedAt = seedDate },
                new SubCategory { Id = 17, Name = "Спортивне обладнання", CategoryId = 4, CreatedAt = seedDate },
                new SubCategory { Id = 18, Name = "Іграшки", CategoryId = 4, CreatedAt = seedDate }
            };
            modelBuilder.Entity<SubCategory>().HasData(subCategories);
        }
    }
} 