using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Core.Interfaces
{
    public interface IAnnouncementService
    {
        // Entity Framework methods
        Task<IEnumerable<Announcement>> GetAllAnnouncementsAsync();
        Task<Announcement?> GetAnnouncementByIdAsync(int id);
        Task<IEnumerable<Announcement>> GetAnnouncementsByCategoryAsync(int categoryId, int? subCategoryId = null);
        Task<IEnumerable<Announcement>> GetAnnouncementsByUserAsync(string userId);
        Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync();
        Task<IEnumerable<Announcement>> SearchAnnouncementsAsync(string searchTerm);
        Task<Announcement> CreateAnnouncementAsync(Announcement announcement);
        Task<Announcement> UpdateAnnouncementAsync(Announcement announcement);
        Task DeleteAnnouncementAsync(int id);
        Task<bool> CanUserEditAnnouncementAsync(int announcementId, string userId);
        Task<IEnumerable<Announcement>> GetAnnouncementsByUserStoredProcAsync(string userId);

        // Stored Procedure methods
        Task<Announcement> CreateAnnouncementStoredProcAsync(Announcement announcement);
        Task UpdateAnnouncementStoredProcAsync(Announcement announcement);
        Task DeleteAnnouncementStoredProcAsync(int id);
        Task<Announcement?> GetAnnouncementByIdStoredProcAsync(int id);
        Task<IEnumerable<Announcement>> GetAllAnnouncementsStoredProcAsync();
    }
} 