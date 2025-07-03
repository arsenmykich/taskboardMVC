using AnnouncementBoard.Core.Models;
using AnnouncementBoard.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace AnnouncementBoard.Business.Services
{
    public interface IDataSeedingService
    {
        Task SeedDataAsync();
    }

    public class DataSeedingService : IDataSeedingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DataSeedingService> _logger;

        public DataSeedingService(IUnitOfWork unitOfWork, ILogger<DataSeedingService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task SeedDataAsync()
        {
            try
            {
                // Check if data already exists
                var existingCategories = await _unitOfWork.Categories.GetAllAsync();
                if (existingCategories.Any())
                {
                    _logger.LogInformation("Seed data already exists. Skipping seeding.");
                    return;
                }

                _logger.LogInformation("Starting data seeding...");

                var seedDate = DateTime.UtcNow;

                // Seed Categories
                var categories = new[]
                {
                    new Category { Name = "Побутова техніка", Description = "Техніка для дому", CreatedAt = seedDate },
                    new Category { Name = "Комп'ютерна техніка", Description = "Комп'ютери та комплектуючі", CreatedAt = seedDate },
                    new Category { Name = "Смартфони", Description = "Мобільні телефони", CreatedAt = seedDate },
                    new Category { Name = "Інше", Description = "Різне", CreatedAt = seedDate }
                };

                foreach (var category in categories)
                {
                    await _unitOfWork.Categories.AddAsync(category);
                }
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Categories seeded successfully.");

                // Get seeded categories with their IDs
                var seededCategories = await _unitOfWork.Categories.GetAllAsync();
                var homeAppliances = seededCategories.First(c => c.Name == "Побутова техніка");
                var computers = seededCategories.First(c => c.Name == "Комп'ютерна техніка");
                var smartphones = seededCategories.First(c => c.Name == "Смартфони");
                var other = seededCategories.First(c => c.Name == "Інше");

                // Seed SubCategories
                var subCategories = new[]
                {
                    // Побутова техніка
                    new SubCategory { Name = "Холодильники", Description = "", CategoryId = homeAppliances.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "Пральні машини", Description = "", CategoryId = homeAppliances.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "Бойлери", Description = "", CategoryId = homeAppliances.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "Печі", Description = "", CategoryId = homeAppliances.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "Витяжки", Description = "", CategoryId = homeAppliances.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "Мікрохвильові печі", Description = "", CategoryId = homeAppliances.Id, CreatedAt = seedDate },
                    
                    // Комп'ютерна техніка
                    new SubCategory { Name = "ПК", Description = "", CategoryId = computers.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "Ноутбуки", Description = "", CategoryId = computers.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "Монітори", Description = "", CategoryId = computers.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "Принтери", Description = "", CategoryId = computers.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "Сканери", Description = "", CategoryId = computers.Id, CreatedAt = seedDate },
                    
                    // Смартфони
                    new SubCategory { Name = "Android смартфони", Description = "", CategoryId = smartphones.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "iOS/Apple смартфони", Description = "", CategoryId = smartphones.Id, CreatedAt = seedDate },
                    
                    // Інше
                    new SubCategory { Name = "Одяг", Description = "", CategoryId = other.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "Взуття", Description = "", CategoryId = other.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "Аксесуари", Description = "", CategoryId = other.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "Спортивне обладнання", Description = "", CategoryId = other.Id, CreatedAt = seedDate },
                    new SubCategory { Name = "Іграшки", Description = "", CategoryId = other.Id, CreatedAt = seedDate }
                };

                foreach (var subCategory in subCategories)
                {
                    await _unitOfWork.SubCategories.AddAsync(subCategory);
                }
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("SubCategories seeded successfully.");
                _logger.LogInformation("Data seeding completed successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during data seeding.");
                throw;
            }
        }
    }
} 