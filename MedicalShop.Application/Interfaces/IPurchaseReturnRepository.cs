
using MedicalShop.Domain.Entities;

namespace MedicalShop.Application.Interfaces
{
    public  interface IPurchaseReturnRepository
    {
        Task<int> GetPreviouslyReturnedQuantityAsync(int purchaseDetailId);
        Task<int> GetNextReturnNumberAsync();
        Task AddAsync(PurchaseReturn purchaseReturn);
    }
}
