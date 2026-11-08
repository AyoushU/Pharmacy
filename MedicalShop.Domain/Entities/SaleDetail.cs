using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Domain.Entities
{
    public class SaleDetail
    {
        public int SaleDetailId { get; private set; }

        public int SaleId { get; private set; }

        public int ProductId { get; private set; }

        public int StockId { get; private set; }

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal GSTPercentage { get; private set; }

        public decimal GSTAmount { get; private set; }

        public decimal DiscountAmount { get; private set; }

        public decimal TotalAmount { get; private set; }

        public Sale Sale { get; private set; } = null!;

        public Product Product { get; private set; } = null!;

        public Stock Stock { get; private set; } = null!;

        private SaleDetail()
        {
        }

        public SaleDetail(
            int productId,
            int stockId,
            int quantity,
            decimal unitPrice,
            decimal gstPercentage,
            decimal discountAmount = 0)
        {
            if (productId <= 0)
                throw new ArgumentException("Valid product is required.");

            if (stockId <= 0)
                throw new ArgumentException("Valid stock batch is required.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (unitPrice <= 0)
                throw new ArgumentException("Unit price must be greater than zero.");

            if (gstPercentage < 0 || gstPercentage > 100)
                throw new ArgumentException("Invalid GST percentage.");

            if (discountAmount < 0)
                throw new ArgumentException("Discount cannot be negative.");

            ProductId = productId;
            StockId = stockId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            GSTPercentage = gstPercentage;
            DiscountAmount = discountAmount;

            var taxableAmount = quantity * unitPrice - discountAmount;

            if (taxableAmount < 0)
                throw new ArgumentException("Discount cannot exceed the item amount.");

            GSTAmount = Math.Round(
                taxableAmount * gstPercentage / 100m, 2);

            TotalAmount = taxableAmount + GSTAmount;
        }
    }
}
