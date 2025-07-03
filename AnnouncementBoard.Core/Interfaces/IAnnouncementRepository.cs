using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Core.Interfaces
{
    public interface IAnnouncementRepository : IGenericRepository<Announcement>
    {
        Task<IEnumerable<Announcement>> GetAnnouncementsByCategoryAsync(int categoryId);
        Task<IEnumerable<Announcement>> GetAnnouncementsBySubCategoryAsync(int subCategoryId);
        Task<IEnumerable<Announcement>> GetAnnouncementsByUserAsync(string userId);
        Task<IEnumerable<Announcement>> GetAnnouncementsByUserStoredProcAsync(string userId);
        Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync();
        Task<IEnumerable<Announcement>> GetAnnouncementsWithIncludesAsync();
        Task<IEnumerable<Announcement>> SearchAnnouncementsAsync(string searchTerm);
    }
} 