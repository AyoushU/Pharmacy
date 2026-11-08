using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using MedicalShop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Infrastructure.Persistence.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SaleRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task AddAsync(Sale sale)
        {
            await _applicationDbContext.Sales.AddAsync(sale);
        }

        public async Task<List<Sale>> GetAllAsync()
        {
            return await _applicationDbContext.Sales
            .Include(s => s.Customer)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
        }

        public async Task<Sale?> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Sales
                .Include(s => s.Customer)
                .Include(s => s.SaleDetails)
                    .ThenInclude(d => d.Product)
                .Include(s => s.SaleDetails)
                    .ThenInclude(d => d.Stock)
                .FirstOrDefaultAsync(s => s.SaleId == id);
        }

        public async Task<decimal> GetTodaySalesTotalAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _applicationDbContext.Sales
                .Where(s =>
                    s.SaleDate >= today &&
                    s.SaleDate < tomorrow &&
                    s.Status == "Completed")
                .SumAsync(s => (decimal?)s.NetAmount) ?? 0m;
        }

        public async Task<bool> InvoiceNumberExistsAsync(string invoiceNumber)
        {
            return await _applicationDbContext.Sales
                .AnyAsync(s => s.InvoiceNumber == invoiceNumber);
        }
    }
}
