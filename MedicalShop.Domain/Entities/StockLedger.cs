using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Domain.Entities
{

    public class StockLedger
    {
        public int StockLedgerId { get; private set; }

        public int ProductId { get; private set; }

        public int? StockId { get; private set; }

        public string TransactionType { get; private set; } = string.Empty;

        public int Quantity { get; private set; }

        public int BalanceQuantity { get; private set; }

        public string? ReferenceNumber { get; private set; }

        public DateTime TransactionDate { get; private set; }

        public string? Remarks { get; private set; }

        // Navigation properties
        public Product Product { get; private set; } = null!;

        public Stock? Stock { get; private set; }

        private StockLedger()
        {
        }

        public StockLedger(
        int productId,
        string batchNumber,
        Stock stock,
        string transactionType,
        int quantity,
        int balanceQuantity,
        string? referenceNumber = null,
        string? remarks = null)
        {
            ProductId = productId;
            Stock = stock;
            TransactionType = transactionType;
            Quantity = quantity;
            BalanceQuantity = balanceQuantity;
            ReferenceNumber = referenceNumber;
            Remarks = remarks;
            TransactionDate = DateTime.UtcNow;
        }
    }
}
