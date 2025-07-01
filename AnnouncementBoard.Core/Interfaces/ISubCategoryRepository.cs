using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Core.Interfaces
{
    public interface ISubCategoryRepository : IGenericRepository<SubCategory>
    {
        Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryAsync(int categoryId);
    }
} 