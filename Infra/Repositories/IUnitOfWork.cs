using System;
using System.Threading.Tasks;
using Infra.Data;

namespace Infra.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync();
        Task BeginTransactionAsync();
        Task RollBackAsync();
        Task CommitAsync();
        public IUserTaskRepository UserTaskRepository { get; }
        public IUserRepository UserRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
    }



    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private IUserTaskRepository _userTaskRepository;
        private IUserRepository _userRepository;
        private ICategoryRepository _categoryRepository;

        public IUserTaskRepository UserTaskRepository => _userTaskRepository ??= new UserTaskRepository(_context);
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
        public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);



        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollBackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
