using Microsoft.AspNetCore.Mvc;

namespace MedicalShop.Web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
