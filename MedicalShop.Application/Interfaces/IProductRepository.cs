using MedicalShop.Domain.Entities;


namespace MedicalShop.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(int id);

        Task AddAsync(Product product);

        Task<bool> ProductCodeExistsAsync(string productCode);
        Task<int> GetTotalProductsCountAsync();

        Task SaveChangesAsync();
    }
}
