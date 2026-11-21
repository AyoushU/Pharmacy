namespace MedicalShop.Application.DTOs.SaleReturn
{
    public class SaleReturnRefundDto
    {
        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = "Cash";

        public string? ReferenceNumber { get; set; }

        public string? Remarks { get; set; }
    }
}
