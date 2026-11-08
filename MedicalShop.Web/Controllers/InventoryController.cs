using MedicalShop.Application.Interfaces;
using MedicalShop.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalShop.Web.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InventoryController(IInventoryService inventoryService, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _inventoryService = inventoryService;
            _productRepository = productRepository;
            _unitOfWork=unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var stocks = await _inventoryService.GetAllStockAsync();
            var products = await _productRepository.GetAllAsync();

            ViewBag.Products = products;

            return View(stocks);
        }

        // add astock

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStock(
    int productId,
    string? batchNumber,
    DateTime? expiryDate,
    int quantity,
    decimal purchasePrice,
    decimal sellingPrice)
        {
            if (productId <= 0 ||
                quantity <= 0 ||
                purchasePrice <= 0 ||
                sellingPrice <= 0)
            {
                TempData["Error"] = "Please enter valid stock details.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _inventoryService.AddStockAsync(
                    productId,
                    batchNumber,
                    expiryDate,
                    quantity,
                    purchasePrice,
                    sellingPrice,
                    "ManualStockIn",
                    null,
                    "Manual stock entry");
                await _unitOfWork.SaveChangesAsync();
                TempData["Success"] = "Stock added successfully.";
               
            }

            catch (Exception)
            {
                TempData["Error"] = "Unable to add stock. Please verify the details and try again.";
            }

            return RedirectToAction(nameof(Index));
        }
        // End
    }
}
