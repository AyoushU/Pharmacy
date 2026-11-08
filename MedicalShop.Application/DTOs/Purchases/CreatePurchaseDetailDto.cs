using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.Purchases
{

    public class CreatePurchaseDetailDto
    {
        public int ProductId { get; set; }

        public string? BatchNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int Quantity { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal GSTPercentage { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
