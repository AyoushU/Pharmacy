using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Domain.Entities
{

    public class PurchaseReturn
    {
        public int PurchaseReturnId { get; private set; }

        public string ReturnNumber { get; private set; } = string.Empty;

        public int PurchaseId { get; private set; }

        public DateTime ReturnDate { get; private set; }

        public string? Reason { get; private set; }

        public decimal TotalAmount { get; private set; }

        public decimal GSTAmount { get; private set; }

        public decimal NetAmount { get; private set; }

        public string Status { get; private set; } = "Completed";

        // Navigation properties
        public Purchase Purchase { get; private set; } = null!;

        public ICollection<PurchaseReturnDetail> PurchaseReturnDetails
        { get; private set; } = new List<PurchaseReturnDetail>();

        private PurchaseReturn()
        {
        }

        public PurchaseReturn(
            string returnNumber,
            int purchaseId,
            DateTime returnDate,
            string? reason,
            decimal totalAmount,
            decimal gstAmount,
            decimal netAmount)
        {
            ReturnNumber = returnNumber;
            PurchaseId = purchaseId;
            ReturnDate = returnDate;
            Reason = reason;
            TotalAmount = totalAmount;
            GSTAmount = gstAmount;
            NetAmount = netAmount;
        }
    }

}
