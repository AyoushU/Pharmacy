using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Domain.Entities
{

    public class Stock
    {
        public int StockId { get; private set; }

        public int ProductId { get; private set; }

        public string? BatchNumber { get; private set; }

        public DateTime? ExpiryDate { get; private set; }

        public int Quantity { get; private set; }

        public decimal PurchasePrice { get; private set; }

        public decimal SellingPrice { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime? ModifiedDate { get; private set; }

        // Navigation property
        public Product Product { get; private set; } = null!;

        private Stock()
        {
        }

        public Stock(
            int productId,
            string? batchNumber,
            DateTime? expiryDate,
            int quantity,
            decimal purchasePrice,
            decimal sellingPrice)
        {
            ProductId = productId;
            BatchNumber = batchNumber;
            ExpiryDate = expiryDate;
            Quantity = quantity;
            PurchasePrice = purchasePrice;
            SellingPrice = sellingPrice;
            CreatedDate = DateTime.UtcNow;
        }

        public void IncreaseQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException(
                    "Quantity must be greater than zero.");

            Quantity += quantity;
            ModifiedDate = DateTime.UtcNow;
        }

        public void DecreaseQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException(
                    "Quantity must be greater than zero.");

            if (quantity > Quantity)
                throw new InvalidOperationException(
                    "Insufficient stock.");

            Quantity -= quantity;
            ModifiedDate = DateTime.UtcNow;
        }
    }
}
