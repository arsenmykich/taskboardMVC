using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Core.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(string id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> CreateOrUpdateUserAsync(string email, string name);
        Task<bool> UserExistsAsync(string email);
    }
} 