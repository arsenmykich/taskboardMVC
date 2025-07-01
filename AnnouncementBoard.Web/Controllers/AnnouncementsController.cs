using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AnnouncementBoard.Core.Interfaces;
using AnnouncementBoard.Core.Models;
using AnnouncementBoard.Web.Models.ViewModels;
using System.Security.Claims;

namespace AnnouncementBoard.Web.Controllers
{
    [Authorize]
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementService _announcementService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly ILogger<AnnouncementsController> _logger;

        public AnnouncementsController(
            IAnnouncementService announcementService,
            ICategoryService categoryService,
            IUserService userService,
            ILogger<AnnouncementsController> logger)
        {
            _announcementService = announcementService;
            _categoryService = categoryService;
            _userService = userService;
            _logger = logger;
        }

        // GET: Announcements/MyAnnouncements
        public async Task<IActionResult> MyAnnouncements()
        {
            try
            {
                var userId = await GetCurrentUserIdAsync();
                if (string.IsNullOrEmpty(userId))
                {
                    return Challenge();
                }

                var announcements = await _announcementService.GetAnnouncementsByUserAsync(userId);
                return View(announcements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при завантаженні оголошень користувача");
                return View(new List<Announcement>());
            }
        }

        // GET: Announcements/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var categories = await _categoryService.GetCategoriesWithSubCategoriesAsync();
                var viewModel = new AnnouncementViewModel
                {
                    Categories = categories,
                    Status = true
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при завантаженні форми створення оголошення");
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Announcements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnouncementViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = await GetCurrentUserIdAsync();
                    if (string.IsNullOrEmpty(userId))
                    {
                        return Challenge();
                    }

                    var announcement = new Announcement
                    {
                        Title = model.Title,
                        Description = model.Description,
                        CategoryId = model.CategoryId,
                        SubCategoryId = model.SubCategoryId,
                        UserId = userId,
                        Status = model.Status
                    };

                    await _announcementService.CreateAnnouncementAsync(announcement);
                    TempData["SuccessMessage"] = "Оголошення успішно створено!";
                    return RedirectToAction(nameof(MyAnnouncements));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при створенні оголошення");
                ModelState.AddModelError("", "Виникла помилка при створенні оголошення. Спробуйте ще раз.");
            }

            // Reload categories for the form
            var categories = await _categoryService.GetCategoriesWithSubCategoriesAsync();
            model.Categories = categories;
            return View(model);
        }

        // GET: Announcements/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var announcement = await _announcementService.GetAnnouncementByIdAsync(id);
                if (announcement == null)
                {
                    return NotFound();
                }

                var userId = await GetCurrentUserIdAsync();
                if (!await _announcementService.CanUserEditAnnouncementAsync(id, userId))
                {
                    return Forbid();
                }

                var categories = await _categoryService.GetCategoriesWithSubCategoriesAsync();
                var viewModel = new AnnouncementViewModel
                {
                    Id = announcement.Id,
                    Title = announcement.Title,
                    Description = announcement.Description,
                    CategoryId = announcement.CategoryId,
                    SubCategoryId = announcement.SubCategoryId,
                    Status = announcement.Status,
                    Categories = categories
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при завантаженні форми редагування оголошення {Id}", id);
                return NotFound();
            }
        }

        // POST: Announcements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AnnouncementViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                var userId = await GetCurrentUserIdAsync();
                if (!await _announcementService.CanUserEditAnnouncementAsync(id, userId))
                {
                    return Forbid();
                }

                if (ModelState.IsValid)
                {
                    var announcement = new Announcement
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Description = model.Description,
                        CategoryId = model.CategoryId,
                        SubCategoryId = model.SubCategoryId,
                        Status = model.Status,
                        UserId = userId
                    };

                    await _announcementService.UpdateAnnouncementAsync(announcement);
                    TempData["SuccessMessage"] = "Оголошення успішно оновлено!";
                    return RedirectToAction(nameof(MyAnnouncements));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при оновленні оголошення {Id}", id);
                ModelState.AddModelError("", "Виникла помилка при оновленні оголошення. Спробуйте ще раз.");
            }

            // Reload categories for the form
            var categories = await _categoryService.GetCategoriesWithSubCategoriesAsync();
            model.Categories = categories;
            return View(model);
        }

        // GET: Announcements/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var announcement = await _announcementService.GetAnnouncementByIdAsync(id);
                if (announcement == null)
                {
                    return NotFound();
                }

                var userId = await GetCurrentUserIdAsync();
                if (!await _announcementService.CanUserEditAnnouncementAsync(id, userId))
                {
                    return Forbid();
                }

                return View(announcement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при завантаженні форми видалення оголошення {Id}", id);
                return NotFound();
            }
        }

        // POST: Announcements/Delete via AJAX
        [HttpPost]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            _logger.LogInformation("DeleteAjax викликано для оголошення {Id}", id);
            
            try
            {
                var userId = await GetCurrentUserIdAsync();
                
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Користувач не автентифікований при спробі видалити оголошення {Id}", id);
                    return Json(new { success = false, message = "Користувач не автентифікований" });
                }
                
                if (!await _announcementService.CanUserEditAnnouncementAsync(id, userId))
                {
                    _logger.LogWarning("Користувач {UserId} не має права видаляти оголошення {Id}", userId, id);
                    return Json(new { success = false, message = "У вас немає права видаляти це оголошення" });
                }
                
                await _announcementService.DeleteAnnouncementAsync(id);
                _logger.LogInformation("Оголошення {Id} успішно видалено користувачем {UserId}", id, userId);
                
                return Json(new { success = true, message = "Оголошення успішно видалено!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при видаленні оголошення {Id}", id);
                return Json(new { success = false, message = "Виникла помилка при видаленні оголошення" });
            }
        }



        // POST: Announcements/ToggleStatus
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var userId = await GetCurrentUserIdAsync();
                if (!await _announcementService.CanUserEditAnnouncementAsync(id, userId))
                {
                    return Json(new { success = false, message = "Ви не маєте права редагувати це оголошення" });
                }

                var announcement = await _announcementService.GetAnnouncementByIdAsync(id);
                if (announcement == null)
                {
                    return Json(new { success = false, message = "Оголошення не знайдено" });
                }

                // Переключити статус
                announcement.Status = !announcement.Status;
                announcement.UpdatedDate = DateTime.UtcNow;

                await _announcementService.UpdateAnnouncementAsync(announcement);

                return Json(new { 
                    success = true, 
                    message = announcement.Status ? "Оголошення активовано" : "Оголошення деактивовано",
                    newStatus = announcement.Status
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при зміні статусу оголошення {Id}", id);
                return Json(new { success = false, message = "Виникла помилка при зміні статусу" });
            }
        }

        private async Task<string> GetCurrentUserIdAsync()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return string.Empty;
            }

            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                var name = User.FindFirst(ClaimTypes.Name)?.Value ?? email;
                user = await _userService.CreateOrUpdateUserAsync(email, name);
            }

            return user.Id;
        }
    }
} 