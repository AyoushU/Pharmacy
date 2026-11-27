
namespace MedicalShop.Domain.Entities
{
        public class CustomerPayment
        {
            public int CustomerPaymentId { get; private set; }

            public int CustomerId { get; private set; }

            public DateTime PaymentDate { get; private set; }

            public decimal Amount { get; private set; }

            public string PaymentMethod { get; private set; } = string.Empty;

            public string? ReferenceNumber { get; private set; }

            public string? Remarks { get; private set; }

            public DateTime CreatedDate { get; private set; }

            public Customer Customer { get; private set; } = null!;

            private CustomerPayment()
            {
            }

        public CustomerPayment(
int customerId,
DateTime paymentDate,
decimal amount,
string paymentMethod,
string? referenceNumber = null,
string? remarks = null)
        {
            if (customerId <= 0)
                throw new ArgumentException("Invalid customer.");

            if (amount <= 0)
                throw new ArgumentException("Payment amount must be greater than zero.");

            if (paymentDate.Date > DateTime.Today)
                throw new ArgumentException("Payment date cannot be in the future.");

            var allowedMethods = new[] { "Cash", "UPI", "Card", "Bank Transfer" };

            if (!allowedMethods.Contains(paymentMethod))
                throw new ArgumentException("Invalid payment method.");

            CustomerId = customerId;
            PaymentDate = paymentDate;
            Amount = amount;
            PaymentMethod = paymentMethod;
            ReferenceNumber = string.IsNullOrWhiteSpace(referenceNumber)
                ? null : referenceNumber.Trim();
            Remarks = string.IsNullOrWhiteSpace(remarks)
                ? null : remarks.Trim();
            CreatedDate = DateTime.UtcNow;
        }
    }
    }


