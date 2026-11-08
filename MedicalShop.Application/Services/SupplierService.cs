using MedicalShop.Application.DTOs.Supplier;
using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(
            ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<List<SupplierDto>> GetAllAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();

            return suppliers.Select(x => new SupplierDto
            {
                SupplierId = x.SupplierId,
                SupplierCode = x.SupplierCode,
                SupplierName = x.SupplierName,
                ContactPerson = x.ContactPerson,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address,
                GSTNumber = x.GSTNumber,
                OpeningBalance = x.OpeningBalance,
                IsActive = x.IsActive
            }).ToList();
        }

        public async Task<SupplierDto?> GetByIdAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);

            if (supplier == null)
                return null;

            return new SupplierDto
            {
                SupplierId = supplier.SupplierId,
                SupplierCode = supplier.SupplierCode,
                SupplierName = supplier.SupplierName,
                ContactPerson = supplier.ContactPerson,
                Phone = supplier.Phone,
                Email = supplier.Email,
                Address = supplier.Address,
                GSTNumber = supplier.GSTNumber,
                OpeningBalance = supplier.OpeningBalance,
                IsActive = supplier.IsActive
            };
        }

        public async Task<int> CreateAsync(
            CreateSupplierDto dto)
        {
            var codeExists =
                await _supplierRepository
                    .SupplierCodeExistsAsync(dto.SupplierCode);

            if (codeExists)
            {
                throw new InvalidOperationException(
                    "Supplier code already exists.");
            }

            var supplier = new Supplier(
                dto.SupplierCode,
                dto.SupplierName,
                dto.ContactPerson,
                dto.Phone,
                dto.Email,
                dto.Address,
                dto.GSTNumber,
                dto.OpeningBalance);

            await _supplierRepository.AddAsync(supplier);

            await _supplierRepository.SaveChangesAsync();

            return supplier.SupplierId;
        }
    }
}
