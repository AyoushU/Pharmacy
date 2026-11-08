using MedicalShop.Application.DTOs.Supplier;
namespace MedicalShop.Application.Interfaces
{

    public interface ISupplierService
    {
        Task<List<SupplierDto>> GetAllAsync();

        Task<SupplierDto?> GetByIdAsync(int id);

        Task<int> CreateAsync(CreateSupplierDto dto);
    }
}
