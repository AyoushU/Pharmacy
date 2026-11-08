using MedicalShop.Application.DTOs.Purchases;
using MedicalShop.Application.Interfaces;
using MedicalShop.Web.ViewModels.Purchases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalShop.Web.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseService _purchaseService;
        private readonly ISupplierService _supplierService;
        private readonly IProductService _productService;

        public PurchaseController(
            IPurchaseService purchaseService,
            ISupplierService supplierService,
            IProductService productService)
        {
            _purchaseService = purchaseService;
            _supplierService = supplierService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var purchases = await _purchaseService.GetAllAsync();

            return View(purchases);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new PurchaseCreateViewModel();

            await LoadDropdownsAsync(model);

            return View(model);
        }

        private async Task LoadDropdownsAsync(
            PurchaseCreateViewModel model)
        {
            var suppliers = await _supplierService.GetAllAsync();
            var products = await _productService.GetAllAsync();

            model.Suppliers = suppliers
                .Where(x => x.IsActive)
                .Select(x => new SelectListItem
                {
                    Value = x.SupplierId.ToString(),
                    Text = $"{x.SupplierCode} - {x.SupplierName}"
                })
                .ToList();

            model.Products = products
                .Where(x => x.IsActive)
                .Select(x => new SelectListItem
                {
                    Value = x.ProductId.ToString(),
                    Text = $"{x.ProductCode} - {x.ProductName}"
                })
                .ToList();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model);
                return View(model);
            }

            var dto = new CreatePurchaseDto
            {
                SupplierId = model.SupplierId,
                PurchaseDate = model.PurchaseDate,
                InvoiceNumber = model.InvoiceNumber,
                InvoiceDate = model.InvoiceDate,

                Details = model.Details.Select(x => new CreatePurchaseDetailDto
                {
                    ProductId = x.ProductId,
                    BatchNumber = x.BatchNumber,
                    ExpiryDate = x.ExpiryDate,
                    Quantity = x.Quantity,
                    PurchasePrice = x.PurchasePrice,
                    GSTPercentage = x.GSTPercentage
                }).ToList()
            };

            try
            {
                await _purchaseService.CreateAsync(dto);

                TempData["Success"] = "Purchase created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                await LoadDropdownsAsync(model);

                return View(model);
            }
        }
    }
}

