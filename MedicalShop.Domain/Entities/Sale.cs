using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Domain.Entities
{
    public class Sale
    {
        public int SaleId { get; private set; }

        public string InvoiceNumber { get; private set; } = string.Empty;

        public DateTime SaleDate { get; private set; }

        public int? CustomerId { get; private set; }

        public decimal SubTotal { get; private set; }

        public decimal GSTAmount { get; private set; }

        public decimal DiscountAmount { get; private set; }

        public decimal NetAmount { get; private set; }

        public string PaymentMethod { get; private set; } = "Cash";

        public string Status { get; private set; } = "Completed";

        public DateTime CreatedDate { get; private set; }

        public Customer? Customer { get; private set; }
        public decimal AmountPaid { get; private set; }

        public decimal BalanceDue { get; private set; }

        public string PaymentStatus { get; private set; } = "Unpaid";

        public ICollection<SaleDetail> SaleDetails { get; private set; }
            = new List<SaleDetail>();

        private Sale()
        {
        }

        public Sale(
     string invoiceNumber,
     int? customerId,
     decimal subTotal,
     decimal gstAmount,
     decimal discountAmount,
     string paymentMethod,
     decimal amountPaid = 0)
        {
            if (string.IsNullOrWhiteSpace(invoiceNumber))
                throw new ArgumentException("Invoice number is required.");

            if (subTotal < 0 || gstAmount < 0 || discountAmount < 0)
                throw new ArgumentException("Sale amounts cannot be negative.");

            if (discountAmount > subTotal + gstAmount)
                throw new ArgumentException("Discount cannot exceed the bill amount.");

            InvoiceNumber = invoiceNumber;
            CustomerId = customerId;
            SubTotal = subTotal;
            GSTAmount = gstAmount;
            DiscountAmount = discountAmount;
            NetAmount = subTotal + gstAmount - discountAmount;

            if (amountPaid < 0 || amountPaid > NetAmount)
                throw new ArgumentException(
                    "Amount received must be between zero and the net amount.");

            AmountPaid = amountPaid;
            BalanceDue = NetAmount - AmountPaid;

            PaymentStatus = BalanceDue == 0
                ? "Paid"
                : AmountPaid > 0
                    ? "PartiallyPaid"
                    : "Unpaid";

            PaymentMethod = paymentMethod;
            SaleDate = DateTime.UtcNow;
            CreatedDate = DateTime.UtcNow;
        }
    }
}
