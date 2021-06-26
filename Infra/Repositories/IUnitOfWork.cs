using System;
using System.Threading.Tasks;
using Infra.Data;
using Microsoft.EntityFrameworkCore.Storage;

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
        private IDbContextTransaction _transaction;
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
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
             
        }

        public async Task RollBackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
