using MedicalShop.Domain.Entities;

namespace MedicalShop.Application.Interfaces
{
 
    public interface IPurchaseRepository
    {
        Task<List<Purchase>> GetAllAsync();

        Task<Purchase?> GetByIdAsync(int id);

        Task AddAsync(Purchase purchase);

        Task<bool> PurchaseNumberExistsAsync(string purchaseNumber);

        
    }
}
