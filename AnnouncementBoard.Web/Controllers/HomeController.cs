using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AnnouncementBoard.Web.Models;
using AnnouncementBoard.Web.Models.ViewModels;
using AnnouncementBoard.Core.Interfaces;

namespace AnnouncementBoard.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAnnouncementService _announcementService;
    private readonly ICategoryService _categoryService;

    public HomeController(
        ILogger<HomeController> logger,
        IAnnouncementService announcementService,
        ICategoryService categoryService)
    {
        _logger = logger;
        _announcementService = announcementService;
        _categoryService = categoryService;
    }



    public async Task<IActionResult> Index(string? searchTerm, int? categoryId, int? subCategoryId)
    {
        try
        {
            var announcements = await _announcementService.GetActiveAnnouncementsAsync();
            var categories = await _categoryService.GetCategoriesWithSubCategoriesAsync();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                announcements = await _announcementService.SearchAnnouncementsAsync(searchTerm);
            }
            else if (categoryId.HasValue)
            {
                announcements = await _announcementService.GetAnnouncementsByCategoryAsync(categoryId.Value, subCategoryId);
            }

            var selectedCategory = categoryId.HasValue ? 
                categories.FirstOrDefault(c => c.Id == categoryId.Value) : null;
            var selectedSubCategory = subCategoryId.HasValue ? 
                selectedCategory?.SubCategories.FirstOrDefault(sc => sc.Id == subCategoryId.Value) : null;

            var viewModel = new HomeViewModel
            {
                Announcements = announcements,
                Categories = categories,
                SearchTerm = searchTerm,
                SelectedCategoryId = categoryId,
                SelectedSubCategoryId = subCategoryId,
                SelectedCategoryName = selectedCategory?.Name,
                SelectedSubCategoryName = selectedSubCategory?.Name
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при завантаженні головної сторінки");
            return View(new HomeViewModel());
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var announcement = await _announcementService.GetAnnouncementByIdAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при завантаженні деталей оголошення {Id}", id);
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetSubCategories(int categoryId)
    {
        try
        {
            var subCategories = await _categoryService.GetSubCategoriesByCategoryAsync(categoryId);
            return Json(subCategories.Select(sc => new { value = sc.Id, text = sc.Name }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при завантаженні підкатегорій для категорії {CategoryId}", categoryId);
            return Json(new object[] { });
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
