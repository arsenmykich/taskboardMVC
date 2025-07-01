using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Core.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
    }
} 