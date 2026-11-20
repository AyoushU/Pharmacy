
namespace MedicalShop.Application.DTOs.PurchaseReturn
{
    public class PurchaseReturnDto
    {
        public int PurchaseId { get; set; }

        public string PurchaseNumber { get; set; } = string.Empty;

        public string SupplierName { get; set; } = string.Empty;

        public DateTime PurchaseDate { get; set; }

        public List<PurchaseReturnItemDto> Items { get; set; } = new();
    }
}
