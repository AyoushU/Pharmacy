using System;
using System.Collections.Generic;
using System.Text;


namespace MedicalShop.Domain.Entities
{
    public class SaleReturnRefund
    {
        public int SaleReturnRefundId { get; private set; }

        public int SaleReturnId { get; private set; }

        public DateTime RefundDate { get; private set; }

        public decimal Amount { get; private set; }

        public string PaymentMethod { get; private set; } = string.Empty;

        public string? ReferenceNumber { get; private set; }

        public string? Remarks { get; private set; }

        public SaleReturn SaleReturn { get; private set; } = null!;

        private SaleReturnRefund()
        {
        }

        public SaleReturnRefund(
            int saleReturnId,
            decimal amount,
            string paymentMethod,
            string? referenceNumber = null,
            string? remarks = null)
        {
            if (saleReturnId <= 0)
                throw new ArgumentException("Valid sale return is required.");

            if (amount <= 0)
                throw new ArgumentException("Refund amount must be greater than zero.");

            if (string.IsNullOrWhiteSpace(paymentMethod))
                throw new ArgumentException("Payment method is required.");

            var allowedMethods = new[] { "Cash", "UPI", "BankTransfer" };

            if (!allowedMethods.Contains(paymentMethod))
                throw new ArgumentException("Invalid refund payment method.");

            SaleReturnId = saleReturnId;
            Amount = amount;
            PaymentMethod = paymentMethod;
            ReferenceNumber = referenceNumber;
            Remarks = remarks;
            RefundDate = DateTime.UtcNow;
        }
    }
}

