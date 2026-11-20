using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.PurchaseReturn
{
    public class PurchaseReturnItemDto
    {
        public int PurchaseDetailId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? BatchNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int PurchasedQuantity { get; set; }

        public int PreviouslyReturnedQuantity { get; set; }

        public int AvailableReturnQuantity { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal GSTPercentage { get; set; }

        public int StockId { get; set; }

        public int AvailableStock { get; set; }
    }
}
