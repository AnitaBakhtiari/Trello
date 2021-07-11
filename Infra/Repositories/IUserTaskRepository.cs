using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Infra.Data;
using Infra.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Infra.Repositories
{
    public interface IUserTaskRepository
    {
        Task<IEnumerable<UserTask>> GetListTasks();
        Task<IEnumerable<UserTask>> GetListToDoTasks();
        Task<IEnumerable<UserTask>> GetListToDoTasksByUser(string userId);
        Task<IEnumerable<UserTask>> GetListArchiveTasks(string id);
        Task<IEnumerable<UserTask>> GetListArchiveTasksAdmin(string id);
        Task<IEnumerable<UserTask>> GetWaitingListTasksAdmin(string id);
        Task AddTask(UserTask userTask);
        Task<UserTask> DoTask(int id, string UserId);
        Task<UserTask> ManageTask(int id, string adminId, string status);

    }

    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly ApplicationDbContext _context;


        private readonly IConfiguration _configuration;
        public UserTaskRepository(IConfiguration configuration )
        {
             //_context = context;
           _configuration = configuration;

        }

        public async Task AddTask(UserTask userTask)
        {
            // await _context.UserTasks.AddAsync(userTask);
            // return await _context.ApplicationUsers.SingleOrDefaultAsync(a => a.Email == email);

            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "INSERT INTO UserTasks(Title,Description,Date,Status,CategoryId,UserId,AdminId)" +
                    " VALUES(@Title,@Description,@Date,@Status,@CategoryId,@UserId,@AdminId)";
                await db.QueryAsync(sql,new
                { 
                    userTask.Title,
                    userTask.Description,
                    userTask.Date, 
                    userTask.Status,
                    userTask.CategoryId,
                    userTask.UserId,
                    userTask.AdminId 
                });
            }
        }


        public async Task<IEnumerable<UserTask>> GetListArchiveTasks(string id)
        {
            // return await _context.UserTasks.Where(a => a.UserId == id && a.Status == "Done").ToListAsync();

            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM UserTasks WHERE userId=@Id";
                return await db.QueryAsync<UserTask>(sql, new { @Id = id });
            }
        }

        public async Task<IEnumerable<UserTask>> GetListArchiveTasksAdmin(string id)
        {
            // return await _context.UserTasks.Where(a => a.AdminId == id && a.Status == "Done").ToListAsync();

            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM UserTasks  WHERE AdminId=@AdminId and Status =@Status ";
                return await db.QueryAsync<UserTask>(sql, new { @AdminId = id, @status = "Done" });
       
            }
        }
        public async Task<IEnumerable<UserTask>> GetWaitingListTasksAdmin(string id)
        {
            // return await _context.UserTasks.Where(a => a.AdminId == id && a.Status == "Waiting").ToListAsync();


            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM UserTasks WHERE AdminId=@AdminId and Status =@Status ";
                return await db.QueryAsync<UserTask>(sql, new { @AdminId = id, @status = "Waiting" });
            }
        }

        public async Task<IEnumerable<UserTask>> GetListTasks()
        {
            // return await _context.UserTasks.OrderByDescending(c => c.Date.Date).ThenBy(c => c.Date.TimeOfDay).ToListAsync();

            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM UserTasks ";
                return await db.QueryAsync<UserTask>(sql);
            }
        }

        public async Task<IEnumerable<UserTask>> GetListToDoTasks()
        {
            // return await _context.UserTasks.OrderByDescending(c => c.Date.Date).ThenBy(c => c.Date.TimeOfDay).Where(a => a.Status == "ToDo").ToListAsync();

            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM UserTasks WHERE  Status =@Status ";
                return await db.QueryAsync<UserTask>(sql, new { @status = "ToDo" });
            }
        }

        public async Task<IEnumerable<UserTask>> GetListToDoTasksByUser(string userId)
        {
            //  return await _context.UserTasks.OrderByDescending(c => c.Date.Date).ThenBy(c => c.Date.TimeOfDay)
            //    .Where(a => a.Status == "ToDo" && a.UserId == userId).ToListAsync();


            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM UserTasks WHERE UserId =@UserId and Status =@Status ";
                return await db.QueryAsync<UserTask>(sql, new { UserId = userId, @status = "ToDo" });
            }
        }



        public async Task<UserTask> DoTask(int id, string userId)
        {
            //var task = await _context.UserTasks.Where(a => a.Id == id && a.UserId == userId).FirstOrDefaultAsync();
            //task.Status = "Waitting";
            //return task;

            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "UPDATE UserTasks SET Status=@Status  WHERE Id =@id AND  UserId = @UserId ";
                return await db.QueryFirstOrDefaultAsync<UserTask>(sql, new { @id = id, @UserId = userId, @Status = "Waitting" });
            }
        }


        public async Task<UserTask> ManageTask(int id, string adminId, string status)
        {
            //  var task = await _context.UserTasks.Where(a => a.Id == id && a.AdminId == adminId).FirstOrDefaultAsync();
            //  return task;

            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                // var sql = "UPDATE UserTasks SET Status=@Status  WHERE Id =@id AND  AdminId = @AdminId";
                var sql = "SELECT * FROM UserTasks WHERE Id =@id and AdminId =@AdminId";
                return await db.QuerySingleOrDefaultAsync<UserTask>(sql, new { @id = id, @AdminId = adminId});
            }
        }


    }
}
