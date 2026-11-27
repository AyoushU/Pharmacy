using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MedicalShop.Web.ViewModels.CustomerPayment
{
    public class ReceiveCustomerPaymentViewModel
    {
        [Required(ErrorMessage = "Please select a customer.")]
        public int? CustomerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; } = DateTime.Today;

        [Required]
        [Range(0.01, 999999999)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please select payment method.")]
        public string PaymentMethod { get; set; } = "Cash";

        public string? ReferenceNumber { get; set; }

        public string? Remarks { get; set; }

        public List<SelectListItem> Customers { get; set; } = new();
    }
}
