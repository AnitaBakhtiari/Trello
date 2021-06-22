using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infra.Data;
using Infra.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public interface IUserTaskRepository
    {
        Task<IEnumerable<UserTask>> GetListTasks();
        Task<IEnumerable<UserTask>> GetListArchiveTasks(string Id);
        Task<IEnumerable<UserTask>> GetListArchiveTasksAdmin(string Id);
        Task AddTask(UserTask userTask);

    }

    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly ApplicationDbContext _context;
        public UserTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTask(UserTask userTask)
        {
            await _context.UserTasks.AddAsync(userTask);
        }

        
        public async Task<IEnumerable<UserTask>> GetListArchiveTasks(string Id)
        {
            return await _context.UserTasks.Where(a => a.User.Id == Id && a.Status=="Done").ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetListArchiveTasksAdmin(string Id)
        {
            return await _context.UserTasks.Where(a => a.Admin.Id == Id && a.Status == "Done").ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetListTasks()
        {
            return await _context.UserTasks.OrderByDescending(c => c.Date.Date).ThenBy(c => c.Date.TimeOfDay).ToListAsync();
        }
    }
}
