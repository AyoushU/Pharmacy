using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Domain.Entities
{

    public class Purchase
    {
        public int PurchaseId { get; private set; }

        public string PurchaseNumber { get; private set; } = string.Empty;

        public int SupplierId { get; private set; }

        public DateTime PurchaseDate { get; private set; }

        public string? InvoiceNumber { get; private set; }

        public DateTime? InvoiceDate { get; private set; }

        public decimal TotalAmount { get; private set; }

        public decimal GSTAmount { get; private set; }

        public decimal NetAmount { get; private set; }

        public string Status { get; private set; } = string.Empty;

        // Navigation properties
        public Supplier Supplier { get; private set; } = null!;

        public ICollection<PurchaseDetail> PurchaseDetails { get; private set; }
            = new List<PurchaseDetail>();

        private Purchase()
        {
        }

        public Purchase(
            string purchaseNumber,
            int supplierId,
            DateTime purchaseDate,
            string? invoiceNumber,
            DateTime? invoiceDate,
            decimal totalAmount,
            decimal gstAmount,
            decimal netAmount,
            string status)
        {
            PurchaseNumber = purchaseNumber;
            SupplierId = supplierId;
            PurchaseDate = purchaseDate;
            InvoiceNumber = invoiceNumber;
            InvoiceDate = invoiceDate;
            TotalAmount = totalAmount;
            GSTAmount = gstAmount;
            NetAmount = netAmount;
            Status = status;
        }

        public void Update(
            int supplierId,
            DateTime purchaseDate,
            string? invoiceNumber,
            DateTime? invoiceDate,
            decimal totalAmount,
            decimal gstAmount,
            decimal netAmount,
            string status)
        {
            SupplierId = supplierId;
            PurchaseDate = purchaseDate;
            InvoiceNumber = invoiceNumber;
            InvoiceDate = invoiceDate;
            TotalAmount = totalAmount;
            GSTAmount = gstAmount;
            NetAmount = netAmount;
            Status = status;
        }
    }
}
