using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Core.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync();
        Task<Category?> GetCategoryWithSubCategoriesAsync(int id);
    }
} 