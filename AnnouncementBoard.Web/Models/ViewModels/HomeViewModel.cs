using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Announcement> Announcements { get; set; } = new List<Announcement>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? SelectedCategoryId { get; set; }
        public int? SelectedSubCategoryId { get; set; }
        public string? SelectedCategoryName { get; set; }
        public string? SelectedSubCategoryName { get; set; }
    }
} 