
namespace MedicalShop.Domain.Entities
{
    public class SaleReturnDetail
    {
        public int SaleReturnDetailId { get; private set; }

        public int SaleReturnId { get; private set; }

        public int SaleDetailId { get; private set; }

        public int ProductId { get; private set; }

        public int StockId { get; private set; }

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal GSTPercentage { get; private set; }

        public decimal GSTAmount { get; private set; }

        public decimal DiscountAmount { get; private set; }

        public decimal TotalAmount { get; private set; }

        public SaleReturn SaleReturn { get; private set; } = null!;

        public SaleDetail SaleDetail { get; private set; } = null!;

        public Product Product { get; private set; } = null!;

        public Stock Stock { get; private set; } = null!;

        private SaleReturnDetail()
        {
        }

        public SaleReturnDetail(
            int saleDetailId,
            int productId,
            int stockId,
            int quantity,
            decimal unitPrice,
            decimal gstPercentage,
            decimal gstAmount,
            decimal discountAmount,
            decimal totalAmount)
        {
            if (saleDetailId <= 0)
                throw new ArgumentException("Valid sale detail is required.");

            if (productId <= 0 || stockId <= 0)
                throw new ArgumentException("Valid product and stock are required.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (unitPrice < 0 || gstPercentage < 0 || gstPercentage > 100)
                throw new ArgumentException("Invalid price or GST percentage.");

            if (gstAmount < 0 || discountAmount < 0 || totalAmount < 0)
                throw new ArgumentException("Amounts cannot be negative.");

            SaleDetailId = saleDetailId;
            ProductId = productId;
            StockId = stockId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            GSTPercentage = gstPercentage;
            GSTAmount = gstAmount;
            DiscountAmount = discountAmount;
            TotalAmount = totalAmount;
        }
    }
}

