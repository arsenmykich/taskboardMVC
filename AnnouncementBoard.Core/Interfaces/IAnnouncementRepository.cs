using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Core.Interfaces
{
    public interface IAnnouncementRepository : IGenericRepository<Announcement>
    {
        // Entity Framework methods
        Task<IEnumerable<Announcement>> GetAnnouncementsByCategoryAsync(int categoryId);
        Task<IEnumerable<Announcement>> GetAnnouncementsBySubCategoryAsync(int subCategoryId);
        Task<IEnumerable<Announcement>> GetAnnouncementsByUserAsync(string userId);
        Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync();
        Task<IEnumerable<Announcement>> GetAnnouncementsWithIncludesAsync();
        Task<IEnumerable<Announcement>> SearchAnnouncementsAsync(string searchTerm);
        Task<IEnumerable<Announcement>> GetAnnouncementsByUserStoredProcAsync(string userId);

        // Stored Procedure methods
        Task<Announcement> CreateAnnouncementStoredProcAsync(Announcement announcement);
        Task UpdateAnnouncementStoredProcAsync(Announcement announcement);
        Task DeleteAnnouncementStoredProcAsync(int id);
        Task<Announcement?> GetAnnouncementByIdStoredProcAsync(int id);
        Task<IEnumerable<Announcement>> GetAllAnnouncementsStoredProcAsync();
    }
} 