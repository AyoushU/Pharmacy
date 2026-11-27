using MedicalShop.Application.DTOs.Products;
using MedicalShop.Application.DTOs.SaleReturn;
using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalShop.Application.Services
{
    public class SaleReturnService : ISaleReturnService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleReturnRepository _saleReturnRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IStockLedgerRepository _stockLedgerRepository;
        private readonly ISaleReturnRefundRepository _refundRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SaleReturnService(
            ISaleRepository saleRepository,
            ISaleReturnRepository saleReturnRepository,
            IStockRepository stockRepository,
            IStockLedgerRepository stockLedgerRepository,
            ISaleReturnRefundRepository refundRepository,
            IUnitOfWork unitOfWork)
        {
            _saleRepository = saleRepository;
            _saleReturnRepository = saleReturnRepository;
            _stockRepository = stockRepository;
            _stockLedgerRepository = stockLedgerRepository;
            _refundRepository = refundRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SaleReturnDto?> GetSaleForReturnAsync(int saleId)
        {
            // 1. Fetch original sale with its details
            var sale = await _saleRepository.GetByIdForReturnAsync(saleId);

            if (sale == null)
                return null;

            // 2. Only completed sales can be returned
            if (sale.Status != "Completed")
                throw new InvalidOperationException(
                    "Only completed sales can be returned.");

            // 3. Prepare return DTO
            var dto = new SaleReturnDto
            {
                SaleId = sale.SaleId,
                InvoiceNumber = sale.InvoiceNumber,
                CustomerName = sale.Customer?.CustomerName ?? "Walk-in Customer",
                SaleDate = sale.SaleDate
            };

            // 4. Map sale details
            foreach (var detail in sale.SaleDetails)
            {
                // Get previously returned quantity for this sale item
                var previouslyReturned =
                    await _saleReturnRepository
                        .GetPreviouslyReturnedQuantityAsync(detail.SaleDetailId);

                // Calculate remaining returnable quantity
                var availableReturnQuantity = Math.Max(
                    0,
                    detail.Quantity - previouslyReturned);

                // Add item to DTO
                dto.Items.Add(new SaleReturnItemDto
                {
                    SaleDetailId = detail.SaleDetailId,
                    ProductId = detail.ProductId,
                    ProductName = detail.Product.ProductName,
                    StockId = detail.StockId,
                    BatchNumber = detail.Stock.BatchNumber,
                    ExpiryDate = detail.Stock.ExpiryDate,
                    SoldQuantity = detail.Quantity,
                    PreviouslyReturnedQuantity = previouslyReturned,
                    AvailableReturnQuantity = availableReturnQuantity,
                    AvailableStock = detail.Stock.Quantity,
                    UnitPrice = detail.UnitPrice,
                    GSTPercentage = detail.GSTPercentage,
                    GSTAmount = detail.GSTAmount,
                    DiscountAmount = detail.DiscountAmount
                });
            }

            return dto;
        }

        // End


        public async Task CreateSaleReturnAsync(CreateSaleReturnDto dto)
        {
            // 1. Basic validations

            if (dto.SaleId <= 0)
                throw new ArgumentException("Invalid sale ID.");

            if (dto.Items == null || !dto.Items.Any())
                throw new InvalidOperationException(
                    "At least one item is required for sale return.");

            if (dto.ReturnDate.Date > DateTime.Today)
                throw new InvalidOperationException(
                    "Return date cannot be in the future.");

            if (string.IsNullOrWhiteSpace(dto.Reason))
                throw new InvalidOperationException(
                    "Return reason is required.");

            if (dto.Reason.Length > 500)
                throw new InvalidOperationException(
                    "Return reason cannot exceed 500 characters.");

            if (dto.Items.GroupBy(x => x.SaleDetailId).Any(g => g.Count() > 1))
                throw new InvalidOperationException(
                    "Duplicate sale items are not allowed.");

            if (dto.Items.Any(x => x.SaleDetailId <= 0 || x.Quantity <= 0))
                throw new InvalidOperationException(
                    "Invalid sale item or return quantity.");

            // 2. Fetch original sale

            var sale = await _saleRepository.GetByIdAsync(dto.SaleId);

            if (sale == null)
                throw new InvalidOperationException("Sale invoice not found.");

            if (sale.Status != "Completed")
                throw new InvalidOperationException(
                    "Only completed sales can be returned.");

            if (dto.ReturnDate.Date < sale.SaleDate.Date)
                throw new InvalidOperationException(
                    "Return date cannot be before sale date.");

            // 3. Initialize calculation variables

            var returnDetails = new List<SaleReturnDetail>();

            var stocksToUpdate = new Dictionary<int, Stock>();

            decimal returnSubTotal = 0;
            decimal returnGstAmount = 0;
            decimal returnDiscountAmount = 0;

            // Item-level discount
            decimal totalItemDiscount =
                sale.SaleDetails.Sum(x => x.DiscountAmount);

            // Bill-level discount
            decimal billDiscount =
                sale.DiscountAmount - totalItemDiscount;

            if (billDiscount < 0)
                throw new InvalidOperationException(
                    "Invalid invoice discount calculation.");

            // Bill discount is applied after GST in the existing SaleService.
            // Therefore, distribute it proportionally over the invoice
            // amount after item discounts, including GST.

            decimal totalBillDiscountBase = sale.SaleDetails.Sum(x =>
                (x.Quantity * x.UnitPrice)
                - x.DiscountAmount
                + x.GSTAmount);

            if (billDiscount > 0 && totalBillDiscountBase <= 0)
                throw new InvalidOperationException(
                    "Unable to calculate bill-level discount allocation.");

            // 4. Validate and calculate each returned item

            foreach (var item in dto.Items)
            {
                var saleDetail = sale.SaleDetails
                    .FirstOrDefault(x => x.SaleDetailId == item.SaleDetailId);

                if (saleDetail == null)
                    throw new InvalidOperationException(
                        $"Sale item {item.SaleDetailId} does not belong to this invoice.");

                // Previously returned quantity
                var previouslyReturned =
                    await _saleReturnRepository
                        .GetPreviouslyReturnedQuantityAsync(item.SaleDetailId);

                var remainingQuantity =
                    saleDetail.Quantity - previouslyReturned;

                if (item.Quantity > remainingQuantity)
                    throw new InvalidOperationException(
                        $"Return quantity cannot exceed remaining quantity for product {saleDetail.Product.ProductName}.");

                // Fetch original stock
                var returnStock = await _stockRepository
                    .GetByIdForReturnAsync(saleDetail.StockId);

                if (returnStock == null)
                    throw new InvalidOperationException(
                        $"Stock record not found for product {saleDetail.Product.ProductName}.");

                if (returnStock.ProductId != saleDetail.ProductId)
                    throw new InvalidOperationException(
                        "Stock record does not match the original sale product.");

                stocksToUpdate[returnStock.StockId] = returnStock;

                // 5. Calculate return amounts

                decimal grossAmount =
                    saleDetail.UnitPrice * item.Quantity;

                // Proportional item discount
                decimal itemDiscount = Math.Round(
                    saleDetail.DiscountAmount * item.Quantity / saleDetail.Quantity,
                    2,
                    MidpointRounding.AwayFromZero);

                // Original invoice line amount after item discount and GST
                decimal lineBillDiscountBase =
                    (saleDetail.Quantity * saleDetail.UnitPrice)
                    - saleDetail.DiscountAmount
                    + saleDetail.GSTAmount;

                // This item's share of bill-level discount
                decimal lineBillDiscount = totalBillDiscountBase > 0
                    ? Math.Round(
                        billDiscount * lineBillDiscountBase / totalBillDiscountBase,
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;

                // Bill discount applicable to returned quantity
                decimal billDiscountForReturn = Math.Round(
                    lineBillDiscount * item.Quantity / saleDetail.Quantity,
                    2,
                    MidpointRounding.AwayFromZero);

                // Proportional GST
                decimal gstAmount = Math.Round(
                    saleDetail.GSTAmount * item.Quantity / saleDetail.Quantity,
                    2,
                    MidpointRounding.AwayFromZero);

                // Total discount
                decimal discountAmount =
                    itemDiscount + billDiscountForReturn;

                // Final return amount
                decimal totalAmount =
                    grossAmount - discountAmount + gstAmount;

                if (totalAmount < 0)
                    throw new InvalidOperationException(
                        "Calculated return amount cannot be negative.");

                // Accumulate totals
                returnSubTotal += grossAmount;
                returnGstAmount += gstAmount;
                returnDiscountAmount += discountAmount;

                // 6. Create return detail

                var returnDetail = new SaleReturnDetail(
                    saleDetailId: saleDetail.SaleDetailId,
                    productId: saleDetail.ProductId,
                    stockId: saleDetail.StockId,
                    quantity: item.Quantity,
                    unitPrice: saleDetail.UnitPrice,
                    gstPercentage: saleDetail.GSTPercentage,
                    gstAmount: gstAmount,
                    discountAmount: discountAmount,
                    totalAmount: totalAmount
                );

                returnDetails.Add(returnDetail);
            }

            // 7. Calculate final return amount

            decimal returnNetAmount =
                returnSubTotal + returnGstAmount - returnDiscountAmount;

            // 8. Validate refund entries

            var refunds = dto.Refunds ?? new List<SaleReturnRefundDto>();

            foreach (var refund in refunds)
            {
                if (refund.Amount <= 0)
                    throw new InvalidOperationException(
                        "Refund amount must be greater than zero.");

                if (refund.PaymentMethod != "Cash" &&
                    refund.PaymentMethod != "UPI" &&
                    refund.PaymentMethod != "BankTransfer")
                {
                    throw new InvalidOperationException(
                        "Invalid refund payment method.");
                }
            }

            decimal totalRefund = refunds.Sum(x => x.Amount);

            if (totalRefund > returnNetAmount)
                throw new InvalidOperationException(
                    "Refund amount cannot exceed return amount.");

            // 9. Generate return number

            var sequenceNumber =
                await _saleReturnRepository.GetNextReturnNumberAsync();

            var returnNumber =
                $"SR-{dto.ReturnDate.Year}-{sequenceNumber:D6}";

            // 10. Create parent SaleReturn

            var saleReturn = new SaleReturn(
                returnNumber,
                sale.SaleId,
                dto.ReturnDate,
                dto.Reason,
                returnSubTotal,
                returnGstAmount,
                returnNetAmount
            );

            // Attach child details
            foreach (var detail in returnDetails)
            {
                saleReturn.SaleReturnDetails.Add(detail);
            }

            // 11. Database transaction

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // 12. Increase stock and create ledger entries

                foreach (var detail in returnDetails)
                {
                    var returnStock = stocksToUpdate[detail.StockId];

                    returnStock.IncreaseQuantity(detail.Quantity);

                    var ledger = new StockLedger(
                        detail.ProductId,
                        returnStock.BatchNumber,
                        returnStock,
                        "SaleReturn",
                        detail.Quantity,
                        returnStock.Quantity,
                        returnNumber,
                        "Customer product return"
                    );

                    await _stockLedgerRepository.AddAsync(ledger);
                }

                // 13. Save SaleReturn and SaleReturnDetails

                await _saleReturnRepository.AddAsync(saleReturn);

                await _unitOfWork.SaveChangesAsync();

                // 14. Process customer refunds

                foreach (var refund in refunds)
                {
                    saleReturn.RecordRefund(refund.Amount);

                    var refundEntity = new SaleReturnRefund(
                        saleReturn.SaleReturnId,
                        refund.Amount,
                        refund.PaymentMethod,
                        refund.ReferenceNumber,
                        refund.Remarks
                    );

                    await _refundRepository.AddAsync(refundEntity);
                }

                // 15. Save refund records

                await _unitOfWork.SaveChangesAsync();

                // 16. Commit transaction

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await _unitOfWork.RollbackTransactionAsync();

                throw new InvalidOperationException(
                    "Stock was modified by another user. Please refresh and try again.");
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<SaleReturnListDto>> GetAllSalesAsync()
        {
            var sales = await _saleReturnRepository.GetAllSalesAsync();
            // first get all product code

            // map the products with productDto
            return sales.Select(x => new SaleReturnListDto
            {
                SaleId = x.SaleId,
                InvoiceNumber = x.InvoiceNumber,
                CustomerName = x.Customer?.CustomerName ?? "Walk-in Customer",
                SaleDate = x.SaleDate,
                NetAmount = x.NetAmount,
                PaymentStatus = x.PaymentStatus,
                Status = x.Status,

            }).ToList();
        }

       
    }
}
