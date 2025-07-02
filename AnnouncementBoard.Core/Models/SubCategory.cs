using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnnouncementBoard.Core.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
    }
} 