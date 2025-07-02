using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Web.Models.ViewModels
{
    public class UserDashboardViewModel
    {
        public User User { get; set; } = null!;
        public IEnumerable<Announcement> UserAnnouncements { get; set; } = new List<Announcement>();
        public IEnumerable<Announcement> RecentAnnouncements { get; set; } = new List<Announcement>();
        public int TotalAnnouncements { get; set; }
        public int ActiveAnnouncements { get; set; }
        public int InactiveAnnouncements { get; set; }
    }
} 