using MedicalShop.Application.DTOs.Dashboard;
using MedicalShop.Application.Interfaces;


namespace MedicalShop.Application.Services
{

        public class DashboardService : IDashboardService
        {
            private readonly IProductRepository _productRepository;
            private readonly IStockRepository _stockRepository;
            private readonly ISaleRepository _saleRepository;

            public DashboardService(
                IProductRepository productRepository,
                IStockRepository stockRepository,
                ISaleRepository saleRepository)
            {
                _productRepository = productRepository;
                _stockRepository = stockRepository;
                _saleRepository = saleRepository;
            }

            public async Task<DashboardDto> GetDashboardDataAsync()
            {
                var dashboard = new DashboardDto
                {
                    TodaySales = await _saleRepository
                        .GetTodaySalesTotalAsync(),

                    TotalProducts = await _productRepository
                        .GetTotalProductsCountAsync(),

                    LowStockCount = await _stockRepository
                        .GetLowStockCountAsync(),

                    NearExpiryCount = await _stockRepository
                        .GetNearExpiryCountAsync(90)
                };

                return dashboard;
            }
        }
    }

