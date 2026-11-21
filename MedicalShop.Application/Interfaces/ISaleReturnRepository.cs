using MedicalShop.Domain.Entities;

namespace MedicalShop.Application.Interfaces
{
    public interface ISaleReturnRepository
    {
        Task<int> GetPreviouslyReturnedQuantityAsync(int saleDetailId);

        Task<int> GetNextReturnNumberAsync();

        Task AddAsync(SaleReturn saleReturn);

        Task<List<Sale>> GetAllSalesAsync();
    }
}
