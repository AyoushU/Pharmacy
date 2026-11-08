using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Domain.Entities
{

    public class PurchaseDetail
    {
        public int PurchaseDetailId { get; private set; }

        public int PurchaseId { get; private set; }

        public int ProductId { get; private set; }

        public string? BatchNumber { get; private set; }

        public DateTime? ExpiryDate { get; private set; }

        public int Quantity { get; private set; }

        public decimal PurchasePrice { get; private set; }

        public decimal GSTPercentage { get; private set; }

        public decimal TotalAmount { get; private set; }

        // Navigation properties
        public Purchase Purchase { get; private set; } = null!;

        public Product Product { get; private set; } = null!;

        private PurchaseDetail()
        {
        }

        public PurchaseDetail(
            int purchaseId,
            int productId,
            string? batchNumber,
            DateTime? expiryDate,
            int quantity,
            decimal purchasePrice,
            decimal gstPercentage,
            decimal totalAmount)
        {
            PurchaseId = purchaseId;
            ProductId = productId;
            BatchNumber = batchNumber;
            ExpiryDate = expiryDate;
            Quantity = quantity;
            PurchasePrice = purchasePrice;
            GSTPercentage = gstPercentage;
            TotalAmount = totalAmount;
        }

        public void Update(
            int productId,
            string? batchNumber,
            DateTime? expiryDate,
            int quantity,
            decimal purchasePrice,
            decimal gstPercentage,
            decimal totalAmount)
        {
            ProductId = productId;
            BatchNumber = batchNumber;
            ExpiryDate = expiryDate;
            Quantity = quantity;
            PurchasePrice = purchasePrice;
            GSTPercentage = gstPercentage;
            TotalAmount = totalAmount;
        }
    }
}
