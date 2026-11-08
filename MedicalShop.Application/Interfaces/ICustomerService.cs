using MedicalShop.Application.DTOs.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllAsync();

        Task<CustomerDto?> GetByIdAsync(int id);

        Task<int> CreateAsync(CreateCustomerDto dto);
    }
}
