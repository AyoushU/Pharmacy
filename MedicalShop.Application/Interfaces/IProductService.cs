using MedicalShop.Application.DTOs.Products;

namespace MedicalShop.Application.Interfaces
{

   

    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();

        Task<ProductDto?> GetByIdAsync(int id);

        Task<int> CreateAsync(CreateProductDto dto);
    }
}
