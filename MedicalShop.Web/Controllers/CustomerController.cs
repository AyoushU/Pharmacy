using MedicalShop.Application.DTOs.Customers;
using MedicalShop.Application.Interfaces;
using MedicalShop.Web.ViewModels.Customers;
using Microsoft.AspNetCore.Mvc;

namespace MedicalShop.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(
            ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllAsync();

            return View(customers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CustomerCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CustomerCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = new CreateCustomerDto
            {
                CustomerCode = model.CustomerCode,
                CustomerName = model.CustomerName,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address,
                OpeningBalance = model.OpeningBalance
            };

            try
            {
                await _customerService.CreateAsync(dto);

                TempData["Success"] =
                    "Customer created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(
                    nameof(model.CustomerCode),
                    ex.Message);

                return View(model);
            }
        }
    }
}
