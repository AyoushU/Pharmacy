using System.Linq;
using System.ComponentModel.DataAnnotations;
namespace MedicalShop.Web.ViewModels.Suppliers
{

    public class SupplierCreateViewModel : IValidatableObject
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Supplier Code")]
        public string SupplierCode { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; } = string.Empty;

        [StringLength(150)]
        [Display(Name = "Contact Person")]
        public string? ContactPerson { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(15)]
        [Display(Name = "GST Number")]
        public string? GSTNumber { get; set; }

        [Range(0, double.MaxValue,
            ErrorMessage = "Opening balance cannot be negative.")]
        [Display(Name = "Opening Balance")]
        public decimal OpeningBalance { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(GSTNumber))
            {
                GSTNumber = GSTNumber.Trim().ToUpper();
            }

            if (!string.IsNullOrWhiteSpace(Phone))
            {
                var phone = Phone.Trim();

                if (!phone.All(char.IsDigit) || phone.Length != 10)
                {
                    yield return new ValidationResult(
                        "Phone number must contain exactly 10 digits.",
                        new[] { nameof(Phone) });
                }
            }
        }
    }
}
