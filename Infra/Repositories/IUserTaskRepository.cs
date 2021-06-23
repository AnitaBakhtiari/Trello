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
        Task<IEnumerable<UserTask>> GetListArchiveTasks(string id);
        Task<IEnumerable<UserTask>> GetListArchiveTasksAdmin(string id);
        Task<IEnumerable<UserTask>> GetWaitingListTasksAdmin(string id);
        Task<string> AddTask(UserTask userTask);
        Task<string> DoTask(int id, string UserId);
        Task<string> ManageTask(int id, string adminId, string status);


    }

    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly ApplicationDbContext _context;
        public UserTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddTask(UserTask userTask)
        {
            await _context.UserTasks.AddAsync(userTask);
            return userTask.UserId;
        }


        public async Task<IEnumerable<UserTask>> GetListArchiveTasks(string id)
        {
            return await _context.UserTasks.Where(a => a.UserId == id && a.Status == "Done").ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetListArchiveTasksAdmin(string id)
        {
            return await _context.UserTasks.Where(a => a.AdminId == id && a.Status == "Done").ToListAsync();
        }
        public async Task<IEnumerable<UserTask>> GetWaitingListTasksAdmin(string id)
        {
            return await _context.UserTasks.Where(a => a.AdminId == id && a.Status == "Waiting").ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetListTasks()
        {
            return await _context.UserTasks.OrderByDescending(c => c.Date.Date).ThenBy(c => c.Date.TimeOfDay).ToListAsync();
        }

        public async Task<string> DoTask(int id, string userId)
        {
            var task = await _context.UserTasks.Where(a => a.Id == id && a.UserId == userId).FirstOrDefaultAsync();
            task.Status = "Waitting";

            return task.UserId;

        }

        public async Task<string> ManageTask(int id, string adminId, string status)
        {
            var task = await _context.UserTasks.Where(a => a.Id == id && a.AdminId == adminId).FirstOrDefaultAsync();
            if (status == "DoAgain")
            {
                task.Status = "DoAgain";
                task.Date = task.Date.AddDays(10);
            }
            else
                task.Status = "Done";

            return task.UserId;
        }

     
    }
}
