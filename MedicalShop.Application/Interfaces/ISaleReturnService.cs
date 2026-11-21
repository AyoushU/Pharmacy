
using MedicalShop.Application.DTOs.SaleReturn;
using MedicalShop.Domain.Entities;

namespace MedicalShop.Application.Interfaces
{
    public interface ISaleReturnService
    {
        Task<SaleReturnDto?> GetSaleForReturnAsync(int saleId);

        Task CreateSaleReturnAsync(CreateSaleReturnDto dto);

        Task<List<SaleReturnListDto>> GetAllSalesAsync();
        
    }
}
