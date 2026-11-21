using MedicalShop.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalShop.Web.Controllers
{
    public class StockController : Controller
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }
        public async Task<IActionResult> Index()
        {
            var stock = await _stockService.GetAllStockListAsync();
            return View(stock);
        }
    }
}
