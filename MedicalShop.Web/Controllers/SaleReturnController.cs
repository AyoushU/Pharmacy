using MedicalShop.Application.DTOs.PurchaseReturn;
using MedicalShop.Application.DTOs.SaleReturn;
using MedicalShop.Application.Interfaces;
using MedicalShop.Application.Services;
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

        [HttpGet]
        public async Task<IActionResult> Create(int saleId)
        {
            var model = await _saleReturnService
                .GetSaleForReturnAsync(saleId);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSaleReturnDto dto)
        {
            // Ignore products for which return quantity is zero.
            dto.Items = dto.Items
                .Where(x => x.Quantity > 0)
                .ToList();

            if (!dto.Items.Any())
            {
                TempData["Error"] = "Please enter return quantity for at least one product.";

                return RedirectToAction(
                    "Create",
                    new { saleId = dto.SaleId });
            }

            try
            {
                await _saleReturnService.CreateSaleReturnAsync(dto);

                TempData["Success"] = "Sale return created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(
                    "Create",
                    new { saleId = dto.SaleId });
            }
        }

    }
}