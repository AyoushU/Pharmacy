using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using MedicalShop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MedicalShop.Infrastructure.Persistence.Repositories
{

    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .OrderByDescending(x => x.CustomerId)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(x => x.CustomerId == id);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task<bool> CustomerCodeExistsAsync(
            string customerCode)
        {
            return await _context.Customers
                .AnyAsync(x => x.CustomerCode == customerCode);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
