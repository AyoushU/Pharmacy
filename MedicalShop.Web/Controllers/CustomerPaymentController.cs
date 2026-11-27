using MedicalShop.Application.DTOs.CustomerPayment;
using MedicalShop.Application.Interfaces;
using MedicalShop.Web.ViewModels.CustomerPayment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalShop.Web.Controllers
{
    public class CustomerPaymentController : Controller
    {
        private readonly ICustomerPaymentService _customerPaymentService;
        private readonly ICustomerService _customerService;

        public CustomerPaymentController(
            ICustomerPaymentService customerPaymentService,
            ICustomerService customerService)
        {
            _customerPaymentService = customerPaymentService;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var customers = await _customerService.GetAllAsync();

            var model = new ReceiveCustomerPaymentViewModel
            {
                Customers = customers.Select(c => new SelectListItem
                {
                    Value = c.CustomerId.ToString(),
                    Text = c.CustomerName
                }).ToList(),

                PaymentDate = DateTime.Today,
                PaymentMethod = "Cash"
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReceiveCustomerPaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var customers = await _customerService.GetAllAsync();

                model.Customers = customers.Select(c => new SelectListItem
                {
                    Value = c.CustomerId.ToString(),
                    Text = c.CustomerName
                }).ToList();

                return View(model);
            }

            var dto = new ReceiveCustomerPaymentDto
            {
                CustomerId = model.CustomerId!.Value,
                PaymentDate = model.PaymentDate,
                Amount = model.Amount,
                PaymentMethod = model.PaymentMethod,
                ReferenceNumber = model.ReferenceNumber,
                Remarks = model.Remarks
            };

            await _customerPaymentService.ReceivePaymentAsync(dto);

            TempData["SuccessMessage"] = "Customer payment received successfully.";

            return RedirectToAction(nameof(Create));
        }

    }
}
