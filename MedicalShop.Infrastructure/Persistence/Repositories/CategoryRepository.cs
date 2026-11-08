using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using MedicalShop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;


namespace MedicalShop.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .OrderBy(x => x.CategoryName)
                .ToListAsync();
        }

        public async Task<List<Category>> GetActiveAsync()
        {
            return await _context.Categories
                .Where(x => x.IsActive)
                .OrderBy(x => x.CategoryName)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(x =>
                    x.CategoryId == id);
        }

        public async Task<bool> ExistsAsync(
            string categoryName)
        {
            return await _context.Categories
                .AnyAsync(x =>
                    x.CategoryName.ToLower() ==
                    categoryName.ToLower());
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
