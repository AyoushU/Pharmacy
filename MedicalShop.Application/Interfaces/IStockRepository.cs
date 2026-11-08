using MedicalShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();

        Task<Stock?> GetByIdAsync(int id);

        Task<Stock?> GetByProductAndBatchAsync(int productId,string? batchNumber);

        Task AddAsync(Stock stock);

        Task<Stock?> GetByIdForSaleAsync(int stockId);
        Task<int> GetLowStockCountAsync();

        Task<int> GetNearExpiryCountAsync(int days);
    }
}
