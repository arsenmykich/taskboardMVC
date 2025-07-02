using System.ComponentModel.DataAnnotations;

namespace AnnouncementBoard.Core.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
        public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
    }
} 