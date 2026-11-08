using MedicalShop.Application.DTOs.Products;
using MedicalShop.Application.Interfaces;
using MedicalShop.Web.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalShop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(
            IProductService productService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            // new code
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // new comment added for create in product controller
            var model = new ProductCreateViewModel();

            await LoadCategoriesAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(model);
                return View(model);
            }

            try
            {
                var dto = new CreateProductDto
                {
                    ProductCode = model.ProductCode,
                    ProductName = model.ProductName,
                    CategoryId = model.CategoryId,
                    Manufacturer = model.Manufacturer,
                    BatchNumber = model.BatchNumber,
                    PurchasePrice = model.PurchasePrice,
                    SellingPrice = model.SellingPrice,
                    GSTPercentage = model.GSTPercentage,
                    Quantity = model.Quantity,
                    ReorderLevel = model.ReorderLevel,
                    ExpiryDate = model.ExpiryDate,
                    Description = model.Description
                };

                await _productService.CreateAsync(dto);

                TempData["Success"] = "Product created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                await LoadCategoriesAsync(model);

                return View(model);
            }
        }

        // load category in dropdown list option
        private async Task LoadCategoriesAsync(ProductCreateViewModel model)
        {
            var categories = await _categoryService.GetActiveAsync();

            model.Categories = categories
                .Select(x => new SelectListItem
                {
                    Value = x.CategoryId.ToString(),
                    Text = x.CategoryName
                })
                .ToList();
        }
        //End load category in dropdown list option
    }
}
