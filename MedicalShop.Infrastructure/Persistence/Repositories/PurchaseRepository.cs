using global::MedicalShop.Application.Interfaces;
using global::MedicalShop.Domain.Entities;
using global::MedicalShop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MedicalShop.Infrastructure.Persistence.Repositories
{

    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> GetAllAsync()
        {
            return await _context.Purchases
                .Include(x => x.Supplier)
                .Include(x => x.PurchaseDetails)
                    .ThenInclude(x => x.Product)
                .OrderByDescending(x => x.PurchaseDate)
                .ToListAsync();
        }

        public async Task<Purchase?> GetByIdAsync(int id)
        {
            return await _context.Purchases
                .Include(x => x.Supplier)
                .Include(x => x.PurchaseDetails)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.PurchaseId == id);
        }

        public async Task AddAsync(Purchase purchase)
        {
            await _context.Purchases.AddAsync(purchase);
        }

        public async Task<bool> PurchaseNumberExistsAsync(string purchaseNumber)
        {
            return await _context.Purchases
                .AnyAsync(x => x.PurchaseNumber == purchaseNumber);
        }

        
    }
}
