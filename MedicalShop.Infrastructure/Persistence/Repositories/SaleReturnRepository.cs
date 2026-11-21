using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using MedicalShop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection.Metadata;

namespace MedicalShop.Infrastructure.Persistence.Repositories
{
    public class SaleReturnRepository : ISaleReturnRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SaleReturnRepository(ApplicationDbContext context)
        {
            _applicationDbContext = context;
        }

        public async Task<int> GetPreviouslyReturnedQuantityAsync(int saleDetailId)
        {
            return await _applicationDbContext.SaleReturnDetails
                .Where(x => x.SaleDetailId == saleDetailId)
                .SumAsync(x => (int?)x.Quantity) ?? 0;
        }

        public async Task<int> GetNextReturnNumberAsync()
        {
            var connection = _applicationDbContext.Database.GetDbConnection();

            var shouldClose = connection.State != ConnectionState.Open;

            if (shouldClose)
                await connection.OpenAsync();

            try
            {
                await using var command = connection.CreateCommand();

                command.CommandText =
                    "SELECT NEXT VALUE FOR dbo.SaleReturnNumberSequence";

                command.Transaction = _applicationDbContext.Database
                    .CurrentTransaction?.GetDbTransaction();

                var result = await command.ExecuteScalarAsync();

                return Convert.ToInt32(result);
            }
            finally
            {
                if (shouldClose)
                    await connection.CloseAsync();
            }
        }

        public async Task AddAsync(SaleReturn saleReturn)
        {
            await _applicationDbContext.SaleReturns.AddAsync(saleReturn);
        }

        public async Task<List<Sale>> GetAllSalesAsync()
        {
            return await _applicationDbContext.Sales
             .Include(s => s.Customer)
             .Where(x => x.Status == "Completed")
             .OrderByDescending(x => x.SaleDate)
             .ToListAsync();

        }

       
    }
}
