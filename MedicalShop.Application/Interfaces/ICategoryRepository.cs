using MedicalShop.Domain.Entities;
namespace MedicalShop.Application.Interfaces
{

    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();

        Task<List<Category>> GetActiveAsync();

        Task<Category?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(string categoryName);

        Task AddAsync(Category category);

        Task SaveChangesAsync();
    }
}
