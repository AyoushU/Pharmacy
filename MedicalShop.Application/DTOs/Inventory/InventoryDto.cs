using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.Inventory
{
    

    public class InventoryDto
    {
        public int StockId { get; set; }

        public int ProductId { get; set; }

        public string ProductCode { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public string? BatchNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int Quantity { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }
    }
}
