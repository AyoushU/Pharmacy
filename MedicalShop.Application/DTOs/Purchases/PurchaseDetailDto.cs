using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.Purchases
{


    public class PurchaseDetailDto
    {
        public int PurchaseDetailId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? BatchNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int Quantity { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal GSTPercentage { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
