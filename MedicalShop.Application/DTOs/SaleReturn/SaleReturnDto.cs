namespace MedicalShop.Application.DTOs.SaleReturn
{
    public class SaleReturnDto
    {
        public int SaleId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public DateTime SaleDate { get; set; }

        public List<SaleReturnItemDto> Items { get; set; } = new();
    }
}

