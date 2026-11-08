using MedicalShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();

        Task<Customer?> GetByIdAsync(int id);

        Task AddAsync(Customer customer);

        Task<bool> CustomerCodeExistsAsync(string customerCode);

        Task SaveChangesAsync();
    }
}
