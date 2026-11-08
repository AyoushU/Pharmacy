using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.Purchases
{

    public class PurchaseDto
    {
        public int PurchaseId { get; set; }

        public string PurchaseNumber { get; set; } = string.Empty;

        public int SupplierId { get; set; }

        public string SupplierName { get; set; } = string.Empty;

        public DateTime PurchaseDate { get; set; }

        public string? InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal GSTAmount { get; set; }

        public decimal NetAmount { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<PurchaseDetailDto> Details { get; set; }
            = new();
    }
}
