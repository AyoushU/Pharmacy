
namespace MedicalShop.Application.DTOs.SaleReturn
{
    public class CreateSaleReturnDto
    {
        public int SaleId { get; set; }

        public DateTime ReturnDate { get; set; } = DateTime.Today;

        public string? Reason { get; set; }

        public List<CreateSaleReturnItemDto> Items { get; set; } = new();
        public List<SaleReturnRefundDto> Refunds { get; set; } = new();
    }
}
