

namespace MedicalShop.Application.DTOs.PurchaseReturn
{
    public class CreatePurchaseReturnDto
    {
        public int PurchaseId { get; set; }

        public DateTime ReturnDate { get; set; } = DateTime.Today;

        public string? Reason { get; set; }

        public List<CreatePurchaseReturnItemDto> Items { get; set; } = new();
    }
}
