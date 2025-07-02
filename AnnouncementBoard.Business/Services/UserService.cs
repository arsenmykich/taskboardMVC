using AnnouncementBoard.Core.Interfaces;
using AnnouncementBoard.Core.Models;

namespace AnnouncementBoard.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.Users.GetUserByEmailAsync(email);
        }

        public async Task<User> CreateOrUpdateUserAsync(string email, string name)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email не може бути пустим");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Ім'я користувача не може бути пустим");

            var existingUser = await _unitOfWork.Users.GetUserByEmailAsync(email);
            
            if (existingUser != null)
            {
                // Update existing user
                existingUser.Name = name;
                existingUser.UpdatedAt = DateTime.UtcNow;
                
                _unitOfWork.Users.Update(existingUser);
                await _unitOfWork.SaveChangesAsync();
                
                return existingUser;
            }
            else
            {
                // Create new user
                var newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    Name = name,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Users.AddAsync(newUser);
                await _unitOfWork.SaveChangesAsync();

                return newUser;
            }
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _unitOfWork.Users.UserExistsAsync(email);
        }
    }
} 