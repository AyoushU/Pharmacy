using System;
using System.Collections.Generic;
using System.Text;
using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using MedicalShop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MedicalShop.Infrastructure.Persistence.Repositories
{
   
   

    public class StockLedgerRepository : IStockLedgerRepository
    {
        private readonly ApplicationDbContext _context;

        public StockLedgerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StockLedger>> GetByProductIdAsync(
            int productId)
        {
            return await _context.StockLedgers
                .Include(x => x.Product)
                .Include(x => x.Stock)
                .Where(x => x.ProductId == productId)
                .OrderByDescending(x => x.TransactionDate)
                .ToListAsync();
        }

        public async Task AddAsync(StockLedger stockLedger)
        {
            await _context.StockLedgers.AddAsync(stockLedger);
        }

       
    }
}
