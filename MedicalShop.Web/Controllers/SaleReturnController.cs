using MedicalShop.Application.DTOs.SaleReturn;
using MedicalShop.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalShop.Web.Controllers
{
    public class SaleReturnController : Controller
    {
        private readonly ISaleReturnService _saleReturnService;

        public SaleReturnController(
            ISaleReturnService saleReturnService)
        {
            _saleReturnService = saleReturnService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            var sales = await _saleReturnService.GetAllSalesAsync();
            if (!string.IsNullOrWhiteSpace(search))
            {
                sales = sales
                    .Where(x =>
                        x.InvoiceNumber.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        x.CustomerName.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            return View(sales);
        }

        [HttpGet]
        public async Task<IActionResult> GetSaleForReturn(int saleId)
        {
            if (saleId <= 0)
                return BadRequest("Invalid Sale ID.");

            var sale = await _saleReturnService
                .GetSaleForReturnAsync(saleId);

            if (sale == null)
                return NotFound("Sale invoice not found.");

            return Ok(sale);
        }

    }
}