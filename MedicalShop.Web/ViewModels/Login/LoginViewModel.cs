using System.ComponentModel.DataAnnotations;

namespace MedicalShop.Web.ViewModels.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is requried!")]
        public string username { get; set; }

        [Required(ErrorMessage = "Password is requried!")]
        public string password { get; set; }
    }
}
