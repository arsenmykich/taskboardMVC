using AnnouncementBoard.Core.Interfaces;
using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync()
        {
            return await _unitOfWork.Categories.GetCategoriesWithSubCategoriesAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _unitOfWork.Categories.GetCategoryWithSubCategoriesAsync(id);
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryAsync(int categoryId)
        {
            return await _unitOfWork.SubCategories.GetSubCategoriesByCategoryAsync(categoryId);
        }

        public async Task InitializeCategoriesAsync()
        {
            // This method can be used to initialize categories if they don't exist
            var existingCategories = await _unitOfWork.Categories.GetAllAsync();
            if (!existingCategories.Any())
            {
                // Categories will be seeded via migrations
                // This method can be extended for additional initialization logic
            }
        }
    }
} 