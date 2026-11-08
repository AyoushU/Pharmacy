using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.Purchases
{

    public class CreatePurchaseDto
    {
        public int SupplierId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string? InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal GSTAmount { get; set; }

        public decimal NetAmount { get; set; }

        public List<CreatePurchaseDetailDto> Details { get; set; }
            = new();
    }
}
