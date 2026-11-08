using MedicalShop.Application.DTOs.Supplier;
using MedicalShop.Application.Interfaces;
using MedicalShop.Web.ViewModels.Suppliers;
using Microsoft.AspNetCore.Mvc;

namespace MedicalShop.Web.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(
            ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierService.GetAllAsync();

            return View(suppliers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new SupplierCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            SupplierCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = new CreateSupplierDto
            {
                SupplierCode = model.SupplierCode,
                SupplierName = model.SupplierName,
                ContactPerson = model.ContactPerson,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address,
                GSTNumber = model.GSTNumber,
                OpeningBalance = model.OpeningBalance
            };

            try
            {
                await _supplierService.CreateAsync(dto);

                TempData["Success"] =
                    "Supplier created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException  ex)
            {
                ModelState.AddModelError(
                    nameof(model.SupplierCode),
                    ex.Message);

                return View(model);
            }
        }
    }
}
