using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using MedicalShop.Infrastructure.Persistence.Context;

namespace MedicalShop.Infrastructure.Persistence.Repositories
{
    public class SaleReturnRefundRepository : ISaleReturnRefundRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleReturnRefundRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SaleReturnRefund refund)
        {
            await _context.SaleReturnRefunds.AddAsync(refund);
        }
    }
}
