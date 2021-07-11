using System;
using System.Data;
using System.Threading.Tasks;
using Infra.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

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

        private IDbConnection db;
        private readonly IConfiguration _configuration;
        private IDbContextTransaction _transaction;
        public UnitOfWork(ApplicationDbContext context , IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        private IUserTaskRepository _userTaskRepository;
        private IUserRepository _userRepository;
        private ICategoryRepository _categoryRepository;

        public IUserTaskRepository UserTaskRepository => _userTaskRepository ??= new UserTaskRepository(_configuration);
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_configuration);
        public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_configuration);



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
