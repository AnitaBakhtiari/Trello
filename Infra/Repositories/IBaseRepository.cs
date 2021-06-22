using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> FindAsync(int Id);
        Task<IEnumerable<T>> GetListAsync();
        Task<IEnumerable<T>> GetArchiveListAsync();
        Task AddAsync(T t);
        Task DeleteAsync(int Id);

    }


    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T t)
        {
            await _context.Set<T>().AddAsync(t);
        }

        public async Task DeleteAsync(int Id)
        {
            var removeItem = await _context.Set<T>().FindAsync(Id);
            _context.Remove(removeItem);
        }

        public async Task<T> FindAsync(int Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }

        public Task<IEnumerable<T>> GetArchiveListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            return await _context.Set<T>().ToListAsync();

            //_context.Set<T>().OrderByDescending(c => c.Time.Date).ThenBy(c => c.Time.TimeOfDay);
        }
    }
}
