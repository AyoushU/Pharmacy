using MedicalShop.Application.DTOs.Customers;
using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(
            ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            return customers.Select(x => new CustomerDto
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                CustomerName = x.CustomerName,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address,
                OpeningBalance = x.OpeningBalance,
                IsActive = x.IsActive
            }).ToList();
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer =
                await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return null;

            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                CustomerCode = customer.CustomerCode,
                CustomerName = customer.CustomerName,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                OpeningBalance = customer.OpeningBalance,
                IsActive = customer.IsActive
            };
        }

        public async Task<int> CreateAsync(
            CreateCustomerDto dto)
        {
            var codeExists =
                await _customerRepository
                    .CustomerCodeExistsAsync(dto.CustomerCode);

            if (codeExists)
            {
                throw new InvalidOperationException(
                    "Customer code already exists.");
            }

            var customer = new Customer(
                dto.CustomerCode,
                dto.CustomerName,
                dto.Phone,
                dto.Email,
                dto.Address,
                dto.OpeningBalance);

            await _customerRepository.AddAsync(customer);

            await _customerRepository.SaveChangesAsync();

            return customer.CustomerId;
        }
    }
}
