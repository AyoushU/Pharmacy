using System.ComponentModel.DataAnnotations;

namespace MedicalShop.Web.ViewModels.Sales
{
    public class CreateSaleViewModel
    {
        public int? CustomerId { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = "Cash";

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; set; }
        public decimal AmountReceived { get; set; }

        public List<CreateSaleItemViewModel> Items { get; set; } = new();
    }

    public class CreateSaleItemViewModel
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int StockId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; set; }
        
    }

}

