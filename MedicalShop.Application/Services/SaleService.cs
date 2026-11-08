using MedicalShop.Application.DTOs.CreateSale;
using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;

namespace MedicalShop.Application.Services
{

    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStockLedgerRepository _stockLedgerRepository;

        public SaleService(ISaleRepository saleRepository, IStockRepository stockRepository,
            IUnitOfWork unitOfWork, IStockLedgerRepository stockLedgerRepository)
        {
            _saleRepository = saleRepository;
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
            _stockLedgerRepository = stockLedgerRepository;
        }



        public async Task<int> CreateSaleAsync(CreateSaleDto dto)
        {
            if (dto.Items == null || dto.Items.Count == 0)
                throw new InvalidOperationException(
                    "At least one sale item is required.");

            if (dto.DiscountAmount < 0)
                throw new InvalidOperationException(
                    "Discount cannot be negative.");

            if (string.IsNullOrWhiteSpace(dto.PaymentMethod))
                throw new InvalidOperationException(
                    "Payment method is required.");

            if (dto.Items.Any(x =>
                x.Quantity <= 0 ||
                x.ProductId <= 0 ||
                x.StockId <= 0 ||
                x.DiscountAmount < 0))
            {
                throw new InvalidOperationException(
                    "Invalid sale item details.");
            }

            // A batch should appear only once in a sale request.
            if (dto.Items.GroupBy(x => x.StockId).Any(g => g.Count() > 1))
                throw new InvalidOperationException(
                    "The same stock batch cannot appear multiple times.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                decimal subTotal = 0m;
                decimal totalGst = 0m;
                decimal totalItemDiscount = 0m;

                var saleDetails = new List<SaleDetail>();
                var selectedStocks = new List<(Stock Stock, int Quantity)>();

                foreach (var item in dto.Items)
                {
                    var stock = await _stockRepository
                        .GetByIdForSaleAsync(item.StockId);

                    if (stock == null)
                        throw new InvalidOperationException(
                            $"Stock batch {item.StockId} was not found.");

                    if (stock.ProductId != item.ProductId)
                        throw new InvalidOperationException(
                            "Selected product does not match the stock batch.");

                    if (stock.Quantity < item.Quantity)
                        throw new InvalidOperationException(
                            $"Insufficient stock for {stock.Product.ProductName}.");

                    if (stock.ExpiryDate.HasValue &&
                        stock.ExpiryDate.Value.Date < DateTime.Today)
                    {
                        throw new InvalidOperationException(
                            $"Expired batch cannot be sold: {stock.BatchNumber}.");
                    }

                    if (stock.SellingPrice <= 0)
                        throw new InvalidOperationException(
                            "Stock selling price must be greater than zero.");

                    var product = stock.Product;
                    var gstPercentage = stock.Product.GSTPercentage;
                    var grossAmount = stock.SellingPrice * item.Quantity;

                    if (item.DiscountAmount > grossAmount)
                        throw new InvalidOperationException(
                            "Item discount cannot exceed its gross amount.");

                    var taxableAmount = grossAmount - item.DiscountAmount;

                    //var gstAmount = Math.Round(
                    //    taxableAmount * product.GSTPercentage / 100m,
                    //    2,
                    //    MidpointRounding.AwayFromZero);
                    var gstAmount = Math.Round(
                        (grossAmount - item.DiscountAmount) * gstPercentage / 100m,
                               2,
                         MidpointRounding.AwayFromZero);

                    var detail = new SaleDetail(
                        product.ProductId,
                        stock.StockId,
                        item.Quantity,
                        stock.SellingPrice,
                        product.GSTPercentage,
                        item.DiscountAmount);

                    saleDetails.Add(detail);
                    selectedStocks.Add((stock, item.Quantity));

                    subTotal += grossAmount;
                    totalGst += gstAmount;
                    totalItemDiscount += item.DiscountAmount;
                }

                var billAmountBeforeBillDiscount = subTotal
                    - totalItemDiscount
                    + totalGst;

                if (dto.DiscountAmount > billAmountBeforeBillDiscount)
                    throw new InvalidOperationException(
                        "Bill discount cannot exceed the bill amount.");

                var totalDiscount = totalItemDiscount + dto.DiscountAmount;

                var invoiceNumber = $"SAL-{Guid.NewGuid():N}";

                var sale = new Sale(
                    invoiceNumber,
                    dto.CustomerId,
                    subTotal,
                    totalGst,
                    totalDiscount,
                    dto.PaymentMethod,
                    dto.AmountReceived);

                foreach (var detail in saleDetails)
                {
                    sale.SaleDetails.Add(detail);
                }

                await _saleRepository.AddAsync(sale);

                foreach (var selected in selectedStocks)
                {
                    var stock = selected.Stock;
                    var quantity = selected.Quantity;

                    stock.DecreaseQuantity(quantity);

                    var ledger = new StockLedger(
                        stock.ProductId,
                        stock.BatchNumber ?? string.Empty,
                        stock,
                        "Sale",
                        -quantity,
                        stock.Quantity,
                        invoiceNumber,
                        $"Stock sold against invoice {invoiceNumber}");

                    await _stockLedgerRepository.AddAsync(ledger);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return sale.SaleId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }



        public async Task<List<Sale>> GetAllSalesAsync()
        {
            return await _saleRepository.GetAllAsync();
        }

        public async Task<Sale?> GetSaleByIdAsync(int id)
        {
            return await _saleRepository.GetByIdAsync(id);
        }
    }
}
