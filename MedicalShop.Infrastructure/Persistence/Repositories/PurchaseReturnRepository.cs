using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using MedicalShop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MedicalShop.Infrastructure.Persistence.Repositories
{
    public class PurchaseReturnRepository : IPurchaseReturnRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseReturnRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetPreviouslyReturnedQuantityAsync(
            int purchaseDetailId)
        {
            return await _context.PurchaseReturnDetails
                .Where(x => x.PurchaseDetailId == purchaseDetailId)
                .SumAsync(x => (int?)x.Quantity) ?? 0;
        }
        //public async Task<int> GetNextReturnNumberAsync()
        //{
        //    return await _context.Database
        //        .SqlQueryRaw<int>(
        //            "SELECT NEXT VALUE FOR dbo.PurchaseReturnNumberSequence AS Value")
        //        .SingleAsync();
        //}

        public async Task<int> GetNextReturnNumberAsync()
        {
            var connection = _context.Database.GetDbConnection();

            var shouldClose = connection.State !=
                              System.Data.ConnectionState.Open;

            if (shouldClose)
                await connection.OpenAsync();

            try
            {
                await using var command = connection.CreateCommand();

                command.CommandText =
                    "SELECT NEXT VALUE FOR dbo.PurchaseReturnNumberSequence";

                command.Transaction = _context.Database
                    .CurrentTransaction?
                    .GetDbTransaction();

                var result = await command.ExecuteScalarAsync();

                return Convert.ToInt32(result);
            }
            finally
            {
                if (shouldClose)
                    await connection.CloseAsync();
            }
        }
        public async Task AddAsync(PurchaseReturn purchaseReturn)
        {
            await _context.PurchaseReturns.AddAsync(purchaseReturn);
        }
    }
}
