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
        Task<IEnumerable<UserTask>> GetWaitingListTasksAdmin(string Id);
        Task AddTask(UserTask userTask);
        Task DoTask(int Id, string UserId);
        Task DoAgainTask(int Id, string AdminId);

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
            return await _context.UserTasks.Where(a => a.UserId == Id && a.Status == "Done").ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetListArchiveTasksAdmin(string Id)
        {
            return await _context.UserTasks.Where(a => a.AdminId == Id && a.Status == "Done").ToListAsync();
        }
        public async Task<IEnumerable<UserTask>> GetWaitingListTasksAdmin(string Id)
        {
            return await _context.UserTasks.Where(a => a.AdminId == Id && a.Status == "Waiting").ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetListTasks()
        {
            return await _context.UserTasks.OrderByDescending(c => c.Date.Date).ThenBy(c => c.Date.TimeOfDay).ToListAsync();
        }

        public async Task DoTask(int Id, string UserId)
        {
            var task = await _context.UserTasks.Where(a => a.Id == Id && a.UserId == UserId).FirstOrDefaultAsync();
            task.Status = "Waitting";
        }

        public async Task DoAgainTask(int Id, string AdminId)
        {
            var task = await _context.UserTasks.Where(a => a.Id == Id && a.AdminId == AdminId).FirstOrDefaultAsync();
            task.Status = "DoAgain";
            task.Date= task.Date.AddDays(10);
        }
    }
}
