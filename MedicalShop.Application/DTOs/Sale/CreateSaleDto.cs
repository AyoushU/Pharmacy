
namespace MedicalShop.Application.DTOs.Sale
{
    public class CreateSaleDto
    {
        public int? CustomerId { get; set; }

        public string PaymentMethod { get; set; } = "Cash";

        public decimal DiscountAmount { get; set; }
        public decimal AmountReceived { get; set; }

        public List<CreateSaleItemDto> Items { get; set; } = new();
    }

    public class CreateSaleItemDto
    {
        public int ProductId { get; set; }

        public int StockId { get; set; }

        public int Quantity { get; set; }

        public decimal DiscountAmount { get; set; }
    }
}
