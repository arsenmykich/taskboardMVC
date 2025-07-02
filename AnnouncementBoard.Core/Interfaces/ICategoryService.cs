using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryAsync(int categoryId);
        Task InitializeCategoriesAsync();
    }
} 