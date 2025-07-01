using AnnouncementBoard.Core.Interfaces;
using AnnouncementBoard.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace AnnouncementBoard.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AnnouncementBoardDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(
            AnnouncementBoardDbContext context,
            IAnnouncementRepository announcements,
            ICategoryRepository categories,
            ISubCategoryRepository subCategories,
            IUserRepository users)
        {
            _context = context;
            
            // Inject repositories through DI
            Announcements = announcements;
            Categories = categories;
            SubCategories = subCategories;
            Users = users;
        }

        public IAnnouncementRepository Announcements { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public ISubCategoryRepository SubCategories { get; private set; }
        public IUserRepository Users { get; private set; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
} 