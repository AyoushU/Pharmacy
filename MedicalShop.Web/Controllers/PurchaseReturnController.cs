using MedicalShop.Application.DTOs.PurchaseReturn;
using MedicalShop.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalShop.Web.Controllers
{
    public class PurchaseReturnController : Controller
    {
        private readonly IPurchaseReturnService _purchaseReturnService;
        private readonly IPurchaseService _purchaseService;

        public PurchaseReturnController(
            IPurchaseReturnService purchaseReturnService,
            IPurchaseService purchaseService)
        {
            _purchaseReturnService = purchaseReturnService;
            _purchaseService = purchaseService;
        }

        public async Task<IActionResult> Index()
        {
            var purchases = await _purchaseService.GetAllAsync();

            return View(purchases);
        }
        [HttpGet]
        public async Task<IActionResult> Create(int purchaseId)
        {
            var model = await _purchaseReturnService
                .GetPurchaseForReturnAsync(purchaseId);

            if (model == null)
                return NotFound();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePurchaseReturnDto dto)
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
                    new { purchaseId = dto.PurchaseId });
            }

            try
            {
                await _purchaseReturnService.CreatePurchaseReturnAsync(dto);

                TempData["Success"] = "Purchase return created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(
                    "Create",
                    new { purchaseId = dto.PurchaseId });
            }
        }
    }
}
