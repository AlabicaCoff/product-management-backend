using ProductManagement.Api.Data;
using ProductManagement.Api.Models;
using ProductManagement.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = await _dbContext.Categories.AsNoTracking().ToListAsync();
            return categories;
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            var category = await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}
