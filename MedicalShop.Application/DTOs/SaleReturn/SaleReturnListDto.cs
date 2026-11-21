using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.SaleReturn
{
    public class SaleReturnListDto
    {
        public int SaleId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public DateTime SaleDate { get; set; }

        public decimal NetAmount { get; set; }

        public string PaymentStatus { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}
