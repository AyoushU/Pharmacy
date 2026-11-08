using MedicalShop.Domain.Entities;

namespace MedicalShop.Application.Interfaces
{
   
    


    public interface IStockLedgerRepository
    {
        Task<List<StockLedger>> GetByProductIdAsync(int productId);

        Task AddAsync(StockLedger ledger);

        
    }
}
