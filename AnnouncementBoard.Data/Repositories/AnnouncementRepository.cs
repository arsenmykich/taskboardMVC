using AnnouncementBoard.Core.Interfaces;
using AnnouncementBoard.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnouncementBoard.Data.Repositories
{
    public class AnnouncementRepository : GenericRepository<Announcement>, IAnnouncementRepository
    {
        public AnnouncementRepository(AnnouncementBoardDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Announcement>> GetAnnouncementsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .Where(a => a.CategoryId == categoryId)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Announcement>> GetAnnouncementsBySubCategoryAsync(int subCategoryId)
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .Where(a => a.SubCategoryId == subCategoryId)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Announcement>> GetAnnouncementsByUserAsync(string userId)
        {
            return await _dbSet
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync()
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .Where(a => a.Status == true)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Announcement>> GetAnnouncementsWithIncludesAsync()
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Announcement>> SearchAnnouncementsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetActiveAnnouncementsAsync();

            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .Where(a => a.Status == true && 
                           (a.Title.Contains(searchTerm) || 
                            a.Description.Contains(searchTerm) ||
                            a.Category.Name.Contains(searchTerm) ||
                            a.SubCategory.Name.Contains(searchTerm)))
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public override async Task<Announcement?> GetByIdAsync(object id)
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .FirstOrDefaultAsync(a => a.Id == (int)id);
        }
    }
} 