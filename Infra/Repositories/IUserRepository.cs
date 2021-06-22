using System;
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
    }
}
