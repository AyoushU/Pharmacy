using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.Products
{

    public class CreateProductDto
    {
        public string ProductCode { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public string? Manufacturer { get; set; }

        public string? BatchNumber { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal GSTPercentage { get; set; }

        public int Quantity { get; set; }

        public int ReorderLevel { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string? Description { get; set; }
    }
}
