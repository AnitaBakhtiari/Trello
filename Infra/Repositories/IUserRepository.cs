using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infra.Data;
using Infra.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task AddSignalR(string id, string connectionId);
        Task<string> FindConnectionIdAsync(string id);

    }


    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
          return await  _context.ApplicationUsers.SingleOrDefaultAsync(a => a.Email == email);
        }

        public async Task AddSignalR(string id, string connectionId)
        {
            var user  = await _context.ApplicationUsers.FindAsync(id);
            user.ConnectionId = connectionId;

        }

        public async Task<string> FindConnectionIdAsync(string id)
        {
           var result= await _context.ApplicationUsers.FindAsync(id);
            return result.ConnectionId;
        }
    }
}
