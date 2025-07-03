using AnnouncementBoard.Core.Interfaces;
using AnnouncementBoard.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace AnnouncementBoard.Data.Repositories
{
    public class AnnouncementRepository : GenericRepository<Announcement>, IAnnouncementRepository
    {
        public AnnouncementRepository(AnnouncementBoardDbContext context) : base(context)
        {
        }

        // CRUD operations using stored procedures

        public async Task<Announcement> CreateAnnouncementStoredProcAsync(Announcement announcement)
        {
            var parameters = new[]
            {
                new SqlParameter("@Title", announcement.Title),
                new SqlParameter("@Description", announcement.Description),
                new SqlParameter("@CategoryId", announcement.CategoryId),
                new SqlParameter("@SubCategoryId", announcement.SubCategoryId),
                new SqlParameter("@UserId", announcement.UserId),
                new SqlParameter("@Status", announcement.Status),
                new SqlParameter("@CreatedDate", announcement.CreatedDate),
                new SqlParameter("@UpdatedDate", announcement.UpdatedDate)
            };

            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CreateAnnouncement @Title, @Description, @CategoryId, @SubCategoryId, @UserId, @Status, @CreatedDate, @UpdatedDate",
                parameters);

            // Get the created announcement (this is a simplified approach)
            return announcement;
        }

        public async Task UpdateAnnouncementStoredProcAsync(Announcement announcement)
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", announcement.Id),
                new SqlParameter("@Title", announcement.Title),
                new SqlParameter("@Description", announcement.Description),
                new SqlParameter("@CategoryId", announcement.CategoryId),
                new SqlParameter("@SubCategoryId", announcement.SubCategoryId),
                new SqlParameter("@Status", announcement.Status),
                new SqlParameter("@UpdatedDate", announcement.UpdatedDate)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_UpdateAnnouncement @Id, @Title, @Description, @CategoryId, @SubCategoryId, @Status, @UpdatedDate",
                parameters);
        }

        public async Task DeleteAnnouncementStoredProcAsync(int id)
        {
            var parameter = new SqlParameter("@Id", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_DeleteAnnouncement @Id", parameter);
        }

        public async Task<Announcement?> GetAnnouncementByIdStoredProcAsync(int id)
        {
            var parameter = new SqlParameter("@Id", id);
            return await _context.Announcements
                .FromSqlRaw("EXEC sp_GetAnnouncementById @Id", parameter)
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Announcement>> GetAllAnnouncementsStoredProcAsync()
        {
            return await _context.Announcements
                .FromSqlRaw("EXEC sp_GetAllAnnouncements")
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .ToListAsync();
        }

        // Original Entity Framework methods kept for compatibility
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

        public async Task<IEnumerable<Announcement>> GetAnnouncementsByUserStoredProcAsync(string userId)
        {
            var parameter = new SqlParameter("@UserId", userId);
            return await _context.Announcements
                .FromSqlRaw("EXEC sp_GetUserAnnouncements @UserId", parameter)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .ToListAsync();
        }
    }
} 