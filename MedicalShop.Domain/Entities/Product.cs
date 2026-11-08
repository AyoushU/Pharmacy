using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Domain.Entities
{

    public class Product
    {
        public int ProductId { get; private set; }

        public string ProductCode { get; private set; } = string.Empty;

        public string ProductName { get; private set; } = string.Empty;

        public int CategoryId { get; private set; }

        public string? Manufacturer { get; private set; }

        public string? BatchNumber { get; private set; }

        public decimal PurchasePrice { get; private set; }

        public decimal SellingPrice { get; private set; }

        public decimal GSTPercentage { get; private set; }

        public int Quantity { get; private set; }

        public int ReorderLevel { get; private set; }

        public DateTime? ExpiryDate { get; private set; }

        public string? Description { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime? ModifiedDate { get; private set; }

        public Category Category { get; private set; } = null!;

        private Product()
        {
        }

        public Product(
            string productCode,
            string productName,
            int categoryId,
            decimal purchasePrice,
            decimal sellingPrice,
            decimal gstPercentage,
            int quantity,
            int reorderLevel,
            string? manufacturer = null,
            string? batchNumber = null,
            DateTime? expiryDate = null,
            string? description = null)
        {
            ProductCode = productCode;
            ProductName = productName;
            CategoryId = categoryId;
            PurchasePrice = purchasePrice;
            SellingPrice = sellingPrice;
            GSTPercentage = gstPercentage;
            Quantity = quantity;
            ReorderLevel = reorderLevel;
            Manufacturer = manufacturer;
            BatchNumber = batchNumber;
            ExpiryDate = expiryDate;
            Description = description;

            IsActive = true;
            CreatedDate = DateTime.UtcNow;
        }

        public void Update(
            string productCode,
            string productName,
            int categoryId,
            decimal purchasePrice,
            decimal sellingPrice,
            decimal gstPercentage,
            int quantity,
            int reorderLevel,
            string? manufacturer,
            string? batchNumber,
            DateTime? expiryDate,
            string? description)
        {
            ProductCode = productCode;
            ProductName = productName;
            CategoryId = categoryId;
            PurchasePrice = purchasePrice;
            SellingPrice = sellingPrice;
            GSTPercentage = gstPercentage;
            Quantity = quantity;
            ReorderLevel = reorderLevel;
            Manufacturer = manufacturer;
            BatchNumber = batchNumber;
            ExpiryDate = expiryDate;
            Description = description;

            ModifiedDate = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            ModifiedDate = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            ModifiedDate = DateTime.UtcNow;
        }
    }
}
