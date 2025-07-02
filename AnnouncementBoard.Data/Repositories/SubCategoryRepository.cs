using AnnouncementBoard.Core.Interfaces;
using AnnouncementBoard.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnouncementBoard.Data.Repositories
{
    public class SubCategoryRepository : GenericRepository<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(AnnouncementBoardDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(sc => sc.Category)
                .Where(sc => sc.CategoryId == categoryId)
                .OrderBy(sc => sc.Name)
                .ToListAsync();
        }

        public override async Task<SubCategory?> GetByIdAsync(object id)
        {
            return await _dbSet
                .Include(sc => sc.Category)
                .FirstOrDefaultAsync(sc => sc.Id == (int)id);
        }
    }
} 