using AnnouncementBoard.Core.Interfaces;
using AnnouncementBoard.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnouncementBoard.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AnnouncementBoardDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _dbSet
                .AnyAsync(u => u.Email == email);
        }
    }
} 