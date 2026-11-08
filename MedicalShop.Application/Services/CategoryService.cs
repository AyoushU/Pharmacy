using MedicalShop.Application.DTOs.Categories;
using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(
            ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories =
                await _categoryRepository.GetAllAsync();

            return categories.Select(x => new CategoryDto
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                Description = x.Description,
                IsActive = x.IsActive
            }).ToList();
        }

        public async Task<List<CategoryDto>> GetActiveAsync()
        {
            var categories =
                await _categoryRepository.GetActiveAsync();

            return categories.Select(x => new CategoryDto
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                Description = x.Description,
                IsActive = x.IsActive
            }).ToList();
        }

        public async Task<int> CreateAsync(
            CreateCategoryDto dto)
        {
            var exists =
                await _categoryRepository
                    .ExistsAsync(dto.CategoryName);

            if (exists)
            {
                throw new InvalidOperationException(
                    "Category already exists.");
            }

            var category = new Category(
                dto.CategoryName,
                dto.Description);

            await _categoryRepository.AddAsync(category);

            await _categoryRepository.SaveChangesAsync();

            return category.CategoryId;
        }
    }
}
