using MedicalShop.Application.DTOs.Products;
using MedicalShop.Application.DTOs.Stock;
using MedicalShop.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;

        public StockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
        public async Task<List<StockDto>> GetAllStockListAsync()
        {
            var stocklist=  await _stockRepository.GetAllStockList();
            return stocklist.Select(x => new StockDto
            {
                StockId=x.StockId,
                ProductId = x.ProductId,
                ProductName = x.Product.ProductName,
                BatchNumber = x.BatchNumber,
                ExpiryDate=x.Product.ExpiryDate,
                Quantity= x.Quantity,
                PurchasePrice=x.PurchasePrice,
                SellingPrice = x.SellingPrice,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate


            }).ToList();

            
        }
    }
}
