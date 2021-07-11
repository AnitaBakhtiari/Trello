using System.Data;
using System.Threading.Tasks;
using Dapper;
using Infra.Data;
using Infra.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infra.Repositories
{
    public interface ICategoryRepository
    {
        Task AddCategory(Category category);

    }

    public class CategoryRepository : ICategoryRepository
    {
       
        // private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public CategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public async Task AddCategory(Category category)
        {
            //  await _context.Categories.AddAsync(category);

            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
           
                var sql = "INSERT INTO Categories (Name) VALUES(@Name)";
                await db.QueryAsync<Category>(sql, new { category.Name });
            }

                

        }
    }
}
