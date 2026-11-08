using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MedicalShop.Web.ViewModels.Purchases
{
    public class PurchaseCreateViewModel
    {
        [Required(ErrorMessage = "Please select a supplier.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a supplier.")]
        public int SupplierId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; } = DateTime.Today;

        [StringLength(50)]
        [Display(Name = "Invoice Number")]
        public string? InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Invoice Date")]
        public DateTime? InvoiceDate { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal GSTAmount { get; set; }

        public decimal NetAmount { get; set; }

        public List<PurchaseDetailViewModel> Details { get; set; }
            = new();

        public List<SelectListItem> Suppliers { get; set; }
            = new();

        public List<SelectListItem> Products { get; set; }
            = new();
    }
}
