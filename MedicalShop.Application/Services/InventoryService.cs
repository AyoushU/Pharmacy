using MedicalShop.Application.DTOs.Inventory;
using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using System.Collections;

namespace MedicalShop.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockLedgerRepository _stockLedgerRepository;
        private readonly IProductRepository _productRepository;

        public InventoryService(
            IStockRepository stockRepository,
            IStockLedgerRepository stockLedgerRepository,
            IProductRepository productRepository)
        {
            _stockRepository = stockRepository;
            _stockLedgerRepository = stockLedgerRepository;
            _productRepository = productRepository;
        }

        public async Task<List<InventoryDto>> GetAllStockAsync()
        {
            var stocks = await _stockRepository.GetAllAsync();

            return stocks.Select(stock => new InventoryDto
            {
                StockId = stock.StockId,
                ProductId = stock.ProductId,
                ProductCode = stock.Product.ProductCode,
                ProductName = stock.Product.ProductName,
                BatchNumber = stock.BatchNumber,
                ExpiryDate = stock.ExpiryDate,
                Quantity = stock.Quantity,
                PurchasePrice = stock.PurchasePrice,
                SellingPrice = stock.SellingPrice
            }).ToList();
        }

        public async Task AddStockAsync(
                   int productId,
                  string? batchNumber,
                  DateTime? expiryDate,
                  int quantity,
                   decimal purchasePrice,
                  decimal sellingPrice,
                  string transactionType,
                  string? referenceNumber = null,
                  string? remarks = null)
        {
            if (productId <= 0)
                throw new InvalidOperationException("Please select a product.");

            if (quantity <= 0)
                throw new InvalidOperationException(
                    "Quantity must be greater than zero.");

            if (purchasePrice < 0)
                throw new InvalidOperationException(
                    "Purchase price cannot be negative.");

            if (sellingPrice < 0)
                throw new InvalidOperationException(
                    "Selling price cannot be negative.");

            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new InvalidOperationException(
                    "Selected product was not found.");

            var stock = await _stockRepository
                .GetByProductAndBatchAsync(productId, batchNumber);

            if (stock == null)
            {
                stock = new Stock(
                    productId,
                    batchNumber,
                    expiryDate,
                    quantity,
                    purchasePrice,
                    sellingPrice);

                await _stockRepository.AddAsync(stock);

                // StockId abhi database save hone ke baad milega.
            }
            else
            {
                stock.IncreaseQuantity(quantity);
            }

            

            var balanceQuantity = stock.Quantity;

            var ledger = new StockLedger(
    productId,
    batchNumber,
    stock,
    transactionType,
    quantity,
    stock.Quantity,
    referenceNumber,
    remarks);


            await _stockLedgerRepository.AddAsync(ledger);

            
        }

       

        public Task RemoveStockAsync(int productId, string? batchNumber, int quantity, string transactionType, string? referenceNumber = null, string? remarks = null)
        {
            throw new NotImplementedException();
        }
    }
}
