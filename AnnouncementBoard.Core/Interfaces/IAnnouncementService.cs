using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Core.Interfaces
{
    public interface IAnnouncementService
    {
        Task<IEnumerable<Announcement>> GetAllAnnouncementsAsync();
        Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync();
        Task<Announcement?> GetAnnouncementByIdAsync(int id);
        Task<IEnumerable<Announcement>> GetAnnouncementsByCategoryAsync(int categoryId);
        Task<IEnumerable<Announcement>> GetAnnouncementsBySubCategoryAsync(int subCategoryId);
        Task<IEnumerable<Announcement>> GetAnnouncementsByUserAsync(string userId);
        Task<IEnumerable<Announcement>> GetAnnouncementsByUserStoredProcAsync(string userId);
        Task<IEnumerable<Announcement>> SearchAnnouncementsAsync(string searchTerm);
        Task<Announcement> CreateAnnouncementAsync(Announcement announcement);
        Task<Announcement> UpdateAnnouncementAsync(Announcement announcement);
        Task DeleteAnnouncementAsync(int id);
        Task<bool> CanUserEditAnnouncementAsync(int announcementId, string userId);
    }
} 