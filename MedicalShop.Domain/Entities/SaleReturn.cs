
namespace MedicalShop.Domain.Entities
{
    public class SaleReturn
    {
        public int SaleReturnId { get; private set; }

        public string ReturnNumber { get; private set; } = string.Empty;

        public int SaleId { get; private set; }

        public DateTime ReturnDate { get; private set; }

        public string? Reason { get; private set; }

        public decimal SubTotal { get; private set; }

        public decimal GSTAmount { get; private set; }

        public decimal NetAmount { get; private set; }

        public decimal RefundAmount { get; private set; }

        public decimal PendingRefundAmount { get; private set; }

        public string RefundStatus { get; private set; } = "Pending";

        public string Status { get; private set; } = "Completed";

        public Sale Sale { get; private set; } = null!;

        public ICollection<SaleReturnDetail> SaleReturnDetails
        { get; private set; } = new List<SaleReturnDetail>();

        private SaleReturn()
        {
        }

        public SaleReturn(
            string returnNumber,
            int saleId,
            DateTime returnDate,
            string? reason,
            decimal subTotal,
            decimal gstAmount,
            decimal netAmount)
        {
            if (string.IsNullOrWhiteSpace(returnNumber))
                throw new ArgumentException("Return number is required.");

            if (saleId <= 0)
                throw new ArgumentException("Valid sale is required.");

            if (subTotal < 0 || gstAmount < 0 || netAmount < 0)
                throw new ArgumentException("Return amounts cannot be negative.");

            ReturnNumber = returnNumber;
            SaleId = saleId;
            ReturnDate = returnDate;
            Reason = reason;
            SubTotal = subTotal;
            GSTAmount = gstAmount;
            NetAmount = netAmount;

            PendingRefundAmount = netAmount;
        }
        public void RecordRefund(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException(
                    "Refund amount must be greater than zero.");

            if (RefundAmount + amount > NetAmount)
                throw new InvalidOperationException(
                    "Refund cannot exceed the return amount.");

            RefundAmount += amount;

            PendingRefundAmount = NetAmount - RefundAmount;

            RefundStatus = PendingRefundAmount == 0
                ? "Refunded"
                : "PartiallyRefunded";
        }
    }
}
