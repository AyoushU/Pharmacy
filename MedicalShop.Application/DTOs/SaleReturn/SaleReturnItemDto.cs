
namespace MedicalShop.Application.DTOs.SaleReturn
{
    public class SaleReturnItemDto
    {
        public int SaleDetailId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int StockId { get; set; }

        public string? BatchNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int SoldQuantity { get; set; }

        public int PreviouslyReturnedQuantity { get; set; }

        public int AvailableReturnQuantity { get; set; }

        public int AvailableStock { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal GSTPercentage { get; set; }

        public decimal GSTAmount { get; set; }

        public decimal DiscountAmount { get; set; }
    }
}
