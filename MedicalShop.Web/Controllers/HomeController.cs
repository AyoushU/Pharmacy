using MedicalShop.Application.Interfaces;
using MedicalShop.Web.Models;
using MedicalShop.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedicalShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
      
        public async Task<IActionResult> Index()
        {
            var data = await _dashboardService
                .GetDashboardDataAsync();
            var model = new DashboardViewModel
            {
                TodaySales = data.TodaySales,
                TotalProducts = data.TotalProducts,
                LowStockCount = data.LowStockCount,
                NearExpiryCount = data.NearExpiryCount
            };

            return View(model);
            //return View(dashboardData);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
