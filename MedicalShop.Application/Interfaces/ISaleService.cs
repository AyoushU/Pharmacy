using MedicalShop.Application.DTOs.Sale;
using MedicalShop.Domain.Entities;

namespace MedicalShop.Application.Interfaces
{
   
   

    public interface ISaleService
    {
        //Task<int> CreateSaleAsync(Sale sale);
        Task<int> CreateSaleAsync(CreateSaleDto dto);

        Task<List<SaleDto>> GetAllSalesAsync();

        Task<Sale?> GetSaleByIdAsync(int id);
    }
}
