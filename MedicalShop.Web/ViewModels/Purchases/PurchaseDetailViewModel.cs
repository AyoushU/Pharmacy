using System.ComponentModel.DataAnnotations;

namespace MedicalShop.Web.ViewModels.Purchases
{
    public class PurchaseDetailViewModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a product.")]
        public int ProductId { get; set; }

        public string? BatchNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Purchase price cannot be negative.")]
        public decimal PurchasePrice { get; set; }

        [Range(0, 100, ErrorMessage = "GST must be between 0 and 100.")]
        public decimal GSTPercentage { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
