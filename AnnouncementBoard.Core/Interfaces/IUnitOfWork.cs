namespace AnnouncementBoard.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAnnouncementRepository Announcements { get; }
        ICategoryRepository Categories { get; }
        ISubCategoryRepository SubCategories { get; }
        IUserRepository Users { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
} 