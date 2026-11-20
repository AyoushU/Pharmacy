using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Domain.Entities
{

    public class PurchaseReturnDetail
    {
        public int PurchaseReturnDetailId { get; private set; }

        public int PurchaseReturnId { get; private set; }

        public int PurchaseDetailId { get; private set; }

        public int ProductId { get; private set; }

        public int StockId { get; private set; }

        public int Quantity { get; private set; }

        public decimal PurchasePrice { get; private set; }

        public decimal GSTPercentage { get; private set; }

        public decimal GSTAmount { get; private set; }

        public decimal TotalAmount { get; private set; }

        // Navigation properties
        public PurchaseReturn PurchaseReturn { get; private set; } = null!;

        public PurchaseDetail PurchaseDetail { get; private set; } = null!;

        public Product Product { get; private set; } = null!;

        public Stock Stock { get; private set; } = null!;

        private PurchaseReturnDetail()
        {
        }

        public PurchaseReturnDetail(
            int purchaseReturnId,
            int purchaseDetailId,
            int productId,
            int stockId,
            int quantity,
            decimal purchasePrice,
            decimal gstPercentage,
            decimal gstAmount,
            decimal totalAmount)
        {
            PurchaseReturnId = purchaseReturnId;
            PurchaseDetailId = purchaseDetailId;
            ProductId = productId;
            StockId = stockId;
            Quantity = quantity;
            PurchasePrice = purchasePrice;
            GSTPercentage = gstPercentage;
            GSTAmount = gstAmount;
            TotalAmount = totalAmount;
        }
    }

}
