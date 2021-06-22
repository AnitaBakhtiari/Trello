using System.Threading.Tasks;
using Infra.Data;
using Infra.Models;

namespace Infra.Repositories
{
    public interface ICategoryRepository
    {
        Task AddCategory(Category category);

    }

    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
        }
    }
}
