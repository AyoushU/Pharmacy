using global::MedicalShop.Domain.Entities;
namespace MedicalShop.Application.Interfaces
{

    public interface ISupplierRepository
    {
        Task<List<Supplier>> GetAllAsync();

        Task<Supplier?> GetByIdAsync(int id);

        Task AddAsync(Supplier supplier);

        Task<bool> SupplierCodeExistsAsync(string supplierCode);

        Task SaveChangesAsync();
    }
}
