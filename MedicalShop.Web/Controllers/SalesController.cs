using MedicalShop.Application.DTOs.Sale;
using MedicalShop.Application.Interfaces;
using MedicalShop.Web.ViewModels.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalShop.Web.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IStockRepository _stockRepository;
        private readonly ICustomerService _customerService; 

        public SalesController(ISaleService saleService, IStockRepository stockRepository, ICustomerService customerService)
        {
            _saleService = saleService;
            _stockRepository = stockRepository;
            _customerService = customerService;
        }
        
        public async Task<IActionResult> Index()
        {
            var sales = await _saleService.GetAllSalesAsync();

            return View(sales);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
                return BadRequest();

            var sale = await _saleService.GetSaleByIdAsync(id);

            if (sale == null)
                return NotFound();

            return View(sale);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        { 
            var stocks = await _stockRepository.GetAllAsync();

            var availableStocks = stocks
                .Where(s => s.Quantity > 0 &&
                            (!s.ExpiryDate.HasValue ||
                             s.ExpiryDate.Value.Date >= DateTime.Today))
                .ToList();
            
            ViewBag.Stocks = availableStocks;

            var customers = await _customerService.GetAllAsync();
            var model = new CreateSaleViewModel
            {
                Customers = customers.Select(c => new SelectListItem
                {
                    Value = c.CustomerId.ToString(),
                    Text = c.CustomerName,
                    Selected = c.CustomerName == "Cash"

                }).ToList(),
                CustomerId= customers.FirstOrDefault(c=> c.CustomerName=="Cash")?.CustomerId ?? 0
            };

            return View(model);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSaleViewModel model)
        {
           
            if (!ModelState.IsValid)
            {
                var stocks = await _stockRepository.GetAllAsync();

                ViewBag.Stocks = stocks
                    .Where(s => s.Quantity > 0 &&
                                (!s.ExpiryDate.HasValue ||
                                 s.ExpiryDate.Value.Date >= DateTime.Today))
                    .ToList();

                return View(model);
            }

            try
            {
                var dto = new CreateSaleDto
                {
                    CustomerId = model.CustomerId,
                    PaymentMethod = model.PaymentMethod,
                    DiscountAmount = model.DiscountAmount,
                    AmountReceived = model.AmountReceived,
                    Items = model.Items.Select(item => new CreateSaleItemDto
                    {
                        ProductId = item.ProductId,
                        StockId = item.StockId,
                        Quantity = item.Quantity,
                        DiscountAmount = item.DiscountAmount
                    }).ToList()
                };

                var saleId = await _saleService.CreateSaleAsync(dto);

                TempData["Success"] = "Sale created successfully.";

                return RedirectToAction(nameof(Details), new { id = saleId });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                var stocks = await _stockRepository.GetAllAsync();

                ViewBag.Stocks = stocks
                    .Where(s => s.Quantity > 0 &&
                                (!s.ExpiryDate.HasValue ||
                                 s.ExpiryDate.Value.Date >= DateTime.Today))
                    .ToList();

                return View(model);
            }
        }

    }

}