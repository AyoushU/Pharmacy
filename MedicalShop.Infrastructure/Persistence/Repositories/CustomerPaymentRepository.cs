
using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using MedicalShop.Infrastructure.Persistence;
using MedicalShop.Infrastructure.Persistence.Context;

namespace MedicalShop.Infrastructure.Repositories
{
    public class CustomerPaymentRepository : ICustomerPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerPaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CustomerPayment customerPayment)
        {
            await _context.CustomerPayments.AddAsync(customerPayment);
        }
    }
}
