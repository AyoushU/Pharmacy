using System.ComponentModel.DataAnnotations;
namespace MedicalShop.Web.ViewModels.Customers
{

    public class CustomerCreateViewModel : IValidatableObject
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [Range(0, double.MaxValue,
            ErrorMessage = "Opening balance cannot be negative.")]
        [Display(Name = "Opening Balance")]
        public decimal OpeningBalance { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(Phone))
            {
                var phone = Phone.Trim();

                if (!phone.All(char.IsDigit) ||
                    phone.Length != 10)
                {
                    yield return new ValidationResult(
                        "Phone number must contain exactly 10 digits.",
                        new[] { nameof(Phone) });
                }
            }
        }
    }
}
