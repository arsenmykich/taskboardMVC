using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using AnnouncementBoard.Core.Interfaces;
using AnnouncementBoard.Web.Models.ViewModels;
using System.Security.Claims;

namespace AnnouncementBoard.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAnnouncementService _announcementService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IUserService userService,
            IAnnouncementService announcementService,
            ILogger<AccountController> logger)
        {
            _userService = userService;
            _announcementService = announcementService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult LoginWithGoogle(string? returnUrl = null)
        {
            var redirectUrl = Url.Action("GoogleCallback", "Account", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleCallback(string? returnUrl = null)
        {
            try
            {
                var authenticateResult = await HttpContext.AuthenticateAsync();
                if (!authenticateResult.Succeeded)
                {
                    _logger.LogWarning("Google authentication failed");
                    return RedirectToAction("Login");
                }

                var email = authenticateResult.Principal?.FindFirst(ClaimTypes.Email)?.Value;
                var name = authenticateResult.Principal?.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("No email found in Google authentication result");
                    return RedirectToAction("Login");
                }

                // Create or update user in our database
                await _userService.CreateOrUpdateUserAsync(email, name ?? email);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google authentication callback");
                return RedirectToAction("Login");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                _logger.LogInformation("User logged out successfully");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var userId = await GetCurrentUserIdAsync();
                if (string.IsNullOrEmpty(userId))
                {
                    return Challenge();
                }

                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return Challenge();
                }

                var userAnnouncements = await _announcementService.GetAnnouncementsByUserAsync(userId);
                var announcementsList = userAnnouncements.ToList();

                var viewModel = new UserDashboardViewModel
                {
                    User = user,
                    UserAnnouncements = announcementsList,
                    TotalAnnouncements = announcementsList.Count,
                    ActiveAnnouncements = announcementsList.Count(a => a.Status),
                    InactiveAnnouncements = announcementsList.Count(a => !a.Status)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user dashboard");
                return RedirectToAction("Index", "Home");
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
            return user?.Id ?? string.Empty;
        }
    }
} 