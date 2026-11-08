using MedicalShop.Application.DTOs.Categories;

namespace MedicalShop.Application.Interfaces
{




    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();

        Task<List<CategoryDto>> GetActiveAsync();

        Task<int> CreateAsync(CreateCategoryDto dto);
    }
}
