using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.Stock
{
    public class StockDto
    {
        public int StockId { get;  set; }

        public int ProductId { get;  set; }
        public string? ProductName { get; set; }

        public string? BatchNumber { get;  set; }

        public DateTime? ExpiryDate { get; set; }

        public int Quantity { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get;  set; }
    }
}
