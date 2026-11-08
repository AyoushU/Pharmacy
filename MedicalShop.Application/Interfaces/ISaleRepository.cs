using MedicalShop.Domain.Entities;

namespace MedicalShop.Application.Interfaces
{
    public interface ISaleRepository
    {
        Task<List<Sale>> GetAllAsync();

        Task<Sale?> GetByIdAsync(int id);

        Task<bool> InvoiceNumberExistsAsync(string invoiceNumber);

        Task AddAsync(Sale sale);
        Task<decimal> GetTodaySalesTotalAsync();
    }
}
