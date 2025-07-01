using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnnouncementBoard.Core.Models
{
    public class Announcement
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;
        
        public bool Status { get; set; } = true; // true = активне, false = неактивне
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        
        [ForeignKey("SubCategory")]
        public int SubCategoryId { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        public virtual SubCategory SubCategory { get; set; } = null!;
    }
} 