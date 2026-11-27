
namespace MedicalShop.Application.DTOs.CustomerPayment
{
    public class ReceiveCustomerPaymentDto
    {
        public int CustomerId { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Today;

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string? ReferenceNumber { get; set; }

        public string? Remarks { get; set; }
    }
}
