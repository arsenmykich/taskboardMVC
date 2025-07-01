using AnnouncementBoard.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace AnnouncementBoard.Web.Models.ViewModels
{
    public class AnnouncementViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Заголовок є обов'язковим")]
        [StringLength(200, ErrorMessage = "Заголовок не може бути довшим за 200 символів")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Опис є обов'язковим")]
        [StringLength(2000, ErrorMessage = "Опис не може бути довшим за 2000 символів")]
        public string Description { get; set; } = string.Empty;
        
        public bool Status { get; set; } = true;
        
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        [Required(ErrorMessage = "Категорія є обов'язковою")]
        public int CategoryId { get; set; }
        
        [Required(ErrorMessage = "Підкатегорія є обов'язковою")]
        public int SubCategoryId { get; set; }
        
        public string UserId { get; set; } = string.Empty;
        
        // Navigation properties for display
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        
        // For dropdowns
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<SubCategory>? SubCategories { get; set; }
    }
} 