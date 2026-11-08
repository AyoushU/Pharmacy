using MedicalShop.Application.DTOs.Products;
using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();  // first get all product code

            // map the products with productDto
            return products.Select(x => new ProductDto
            {
                ProductId = x.ProductId,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                Manufacturer = x.Manufacturer,
                BatchNumber = x.BatchNumber,
                PurchasePrice = x.PurchasePrice,
                SellingPrice = x.SellingPrice,
                GSTPercentage = x.GSTPercentage,
                Quantity = x.Quantity,
                ReorderLevel = x.ReorderLevel,
                ExpiryDate = x.ExpiryDate,
                Description = x.Description,
                IsActive = x.IsActive
            }).ToList();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.CategoryName,
                Manufacturer = product.Manufacturer,
                BatchNumber = product.BatchNumber,
                PurchasePrice = product.PurchasePrice,
                SellingPrice = product.SellingPrice,
                GSTPercentage = product.GSTPercentage,
                Quantity = product.Quantity,
                ReorderLevel = product.ReorderLevel,
                ExpiryDate = product.ExpiryDate,
                Description = product.Description,
                IsActive = product.IsActive
            };
        }

        public async Task<int> CreateAsync(CreateProductDto dto)
        {
            var codeExists =
                await _productRepository
                    .ProductCodeExistsAsync(dto.ProductCode);

            if (codeExists)
            {
                throw new InvalidOperationException(
                    "Product code already exists.");
            }

            var product = new Product(
                dto.ProductCode,
                dto.ProductName,
                dto.CategoryId,
                dto.PurchasePrice,
                dto.SellingPrice,
                dto.GSTPercentage,
                dto.Quantity,
                dto.ReorderLevel,
                dto.Manufacturer,
                dto.BatchNumber,
                dto.ExpiryDate,
                dto.Description);

            await _productRepository.AddAsync(product);

            await _productRepository.SaveChangesAsync();

            return product.ProductId;
        }
    }
}