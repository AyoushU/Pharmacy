using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.Sale
{
    public class SaleDto
    {
     public int SaleId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime SaleDate { get; set; }

        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal NetAmount { get;  set; }

        public string? Status { get; set; }
    }
}
