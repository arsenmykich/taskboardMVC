using AnnouncementBoard.Core.Interfaces;
using AnnouncementBoard.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnouncementBoard.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AnnouncementBoardDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync()
        {
            return await _dbSet
                .Include(c => c.SubCategories)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryWithSubCategoriesAsync(int id)
        {
            return await _dbSet
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
} 