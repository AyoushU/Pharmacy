using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using MedicalShop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MedicalShop.Infrastructure.Persistence.Repositories
{
   

    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks
                .Include(x => x.Product)
                .OrderBy(x => x.Product.ProductName)
                .ThenBy(x => x.BatchNumber)
                .ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.StockId == id);
        }

        public async Task<Stock?> GetByProductAndBatchAsync(
            int productId,
            string? batchNumber)
        {
            return await _context.Stocks
                .FirstOrDefaultAsync(x =>
                    x.ProductId == productId &&
                    x.BatchNumber == batchNumber);
        }

        public async Task AddAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
        }
        public async Task<Stock?> GetByIdForSaleAsync(int stockId)
        {
            return await _context.Stocks
                .Include(s => s.Product)
                .FirstOrDefaultAsync(s => s.StockId == stockId);
        }

        public async Task<int> GetLowStockCountAsync()
        {
            return await _context.Products
       .Where(p => _context.Stocks
           .Where(s => s.ProductId == p.ProductId)
           .Sum(s => (int?)s.Quantity) <= p.ReorderLevel)
       .CountAsync();


        }

        public async Task<int> GetNearExpiryCountAsync(int days)
        {
            var today = DateTime.Today;
            var expiryLimit = today.AddDays(days);

            return await _context.Stocks
                .CountAsync(s =>
                    s.Quantity > 0 &&
                    s.ExpiryDate.HasValue &&
                    s.ExpiryDate.Value >= today &&
                    s.ExpiryDate.Value <= expiryLimit);
        }
    }
}
