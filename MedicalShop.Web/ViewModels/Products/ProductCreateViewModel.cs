using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MedicalShop.Web.ViewModels.Products
{
    public class ProductCreateViewModel : IValidatableObject
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a category.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }

        public string? Manufacturer { get; set; }

        [StringLength(50)]
        [Display(Name = "Batch Number")]
        public string? BatchNumber { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Purchase price cannot be negative.")]
        public decimal PurchasePrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Selling price cannot be negative.")]
        public decimal SellingPrice { get; set; }

        [Range(0, 100, ErrorMessage = "GST must be between 0 and 100.")]
        public decimal GSTPercentage { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Reorder level cannot be negative.")]
        public int ReorderLevel { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // Dropdown data
        public List<SelectListItem> Categories { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(
           ValidationContext validationContext)
        {
            if (SellingPrice < PurchasePrice)
            {
                yield return new ValidationResult(
                    "Selling price cannot be less than purchase price.",
                    new[] { nameof(SellingPrice) });
            }

            if (ExpiryDate.HasValue &&
                ExpiryDate.Value.Date <= DateTime.Today)
            {
                yield return new ValidationResult(
                    "Expiry date must be a future date.",
                    new[] { nameof(ExpiryDate) });
            }
        }

    }
}
