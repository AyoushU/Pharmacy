using MedicalShop.Application.DTOs.Purchases;

namespace MedicalShop.Application.Interfaces
{

    public interface IPurchaseService
    {
        Task<List<PurchaseDto>> GetAllAsync();

        Task<PurchaseDto?> GetByIdAsync(int id);

        Task<int> CreateAsync(CreatePurchaseDto dto);
    }
}
