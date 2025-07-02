using AnnouncementBoard.Core.Interfaces;
using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Business.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnnouncementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Announcement>> GetAllAnnouncementsAsync()
        {
            return await _unitOfWork.Announcements.GetAnnouncementsWithIncludesAsync();
        }

        public async Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync()
        {
            return await _unitOfWork.Announcements.GetActiveAnnouncementsAsync();
        }

        public async Task<Announcement?> GetAnnouncementByIdAsync(int id)
        {
            return await _unitOfWork.Announcements.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Announcement>> GetAnnouncementsByCategoryAsync(int categoryId)
        {
            return await _unitOfWork.Announcements.GetAnnouncementsByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<Announcement>> GetAnnouncementsBySubCategoryAsync(int subCategoryId)
        {
            return await _unitOfWork.Announcements.GetAnnouncementsBySubCategoryAsync(subCategoryId);
        }

        public async Task<IEnumerable<Announcement>> GetAnnouncementsByUserAsync(string userId)
        {
            return await _unitOfWork.Announcements.GetAnnouncementsByUserAsync(userId);
        }

        public async Task<IEnumerable<Announcement>> GetAnnouncementsByUserStoredProcAsync(string userId)
        {
            return await _unitOfWork.Announcements.GetAnnouncementsByUserStoredProcAsync(userId);
        }

        public async Task<IEnumerable<Announcement>> SearchAnnouncementsAsync(string searchTerm)
        {
            return await _unitOfWork.Announcements.SearchAnnouncementsAsync(searchTerm);
        }

        public async Task<Announcement> CreateAnnouncementAsync(Announcement announcement)
        {
            // Business logic validation
            if (string.IsNullOrWhiteSpace(announcement.Title))
                throw new ArgumentException("Заголовок оголошення не може бути пустим");

            if (string.IsNullOrWhiteSpace(announcement.Description))
                throw new ArgumentException("Опис оголошення не може бути пустим");

            if (string.IsNullOrWhiteSpace(announcement.UserId))
                throw new ArgumentException("Користувач повинен бути автентифікований");

            // Validate category and subcategory exist
            var category = await _unitOfWork.Categories.GetByIdAsync(announcement.CategoryId);
            if (category == null)
                throw new ArgumentException("Обрана категорія не існує");

            var subCategory = await _unitOfWork.SubCategories.GetByIdAsync(announcement.SubCategoryId);
            if (subCategory == null || subCategory.CategoryId != announcement.CategoryId)
                throw new ArgumentException("Обрана підкатегорія не існує або не належить до обраної категорії");

            // Set timestamps
            announcement.CreatedDate = DateTime.UtcNow;
            announcement.UpdatedDate = DateTime.UtcNow;
            announcement.Status = true; // Active by default

            await _unitOfWork.Announcements.AddAsync(announcement);
            await _unitOfWork.SaveChangesAsync();

            return announcement;
        }

        public async Task<Announcement> UpdateAnnouncementAsync(Announcement announcement)
        {
            var existingAnnouncement = await _unitOfWork.Announcements.GetByIdAsync(announcement.Id);
            if (existingAnnouncement == null)
                throw new ArgumentException("Оголошення не знайдено");

            // Business logic validation
            if (string.IsNullOrWhiteSpace(announcement.Title))
                throw new ArgumentException("Заголовок оголошення не може бути пустим");

            if (string.IsNullOrWhiteSpace(announcement.Description))
                throw new ArgumentException("Опис оголошення не може бути пустим");

            // Validate category and subcategory exist
            var category = await _unitOfWork.Categories.GetByIdAsync(announcement.CategoryId);
            if (category == null)
                throw new ArgumentException("Обрана категорія не існує");

            var subCategory = await _unitOfWork.SubCategories.GetByIdAsync(announcement.SubCategoryId);
            if (subCategory == null || subCategory.CategoryId != announcement.CategoryId)
                throw new ArgumentException("Обрана підкатегорія не існує або не належить до обраної категорії");

            // Update properties
            existingAnnouncement.Title = announcement.Title;
            existingAnnouncement.Description = announcement.Description;
            existingAnnouncement.CategoryId = announcement.CategoryId;
            existingAnnouncement.SubCategoryId = announcement.SubCategoryId;
            existingAnnouncement.Status = announcement.Status;
            existingAnnouncement.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Announcements.Update(existingAnnouncement);
            await _unitOfWork.SaveChangesAsync();

            return existingAnnouncement;
        }

        public async Task DeleteAnnouncementAsync(int id)
        {
            // Використовуємо простий запит з GenericRepository
            var announcement = await _unitOfWork.Announcements.SingleOrDefaultAsync(a => a.Id == id);
            
            if (announcement == null)
                throw new ArgumentException("Оголошення не знайдено");

            _unitOfWork.Announcements.Remove(announcement);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> CanUserEditAnnouncementAsync(int announcementId, string userId)
        {
            // Використовуємо простий запит з GenericRepository
            var announcement = await _unitOfWork.Announcements.SingleOrDefaultAsync(a => a.Id == announcementId);
            return announcement != null && announcement.UserId == userId;
        }
    }
} 