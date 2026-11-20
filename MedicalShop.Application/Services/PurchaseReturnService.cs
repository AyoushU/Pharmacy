using MedicalShop.Application.DTOs.PurchaseReturn;
using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalShop.Application.Services
{
    public class PurchaseReturnService : IPurchaseReturnService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPurchaseReturnRepository _purchaseReturnRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IStockLedgerRepository _stockLedgerRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PurchaseReturnService(IPurchaseRepository purchaseRepository,IPurchaseReturnRepository purchaseReturnRepository,
           IStockRepository stockRepository, IUnitOfWork unitOfWork, IStockLedgerRepository stockLedgerRepository)
        {
            _purchaseRepository = purchaseRepository;
            _purchaseReturnRepository = purchaseReturnRepository;
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
            _stockLedgerRepository = stockLedgerRepository;
        }

        public async Task<PurchaseReturnDto?> GetPurchaseForReturnAsync(
            int purchaseId)
        {
            var purchase = await _purchaseRepository
                .GetByIdForReturnAsync(purchaseId);

            if (purchase == null)
                return null;

            var dto = new PurchaseReturnDto
            {
                PurchaseId = purchase.PurchaseId,
                PurchaseNumber = purchase.PurchaseNumber,
                SupplierName = purchase.Supplier.SupplierName,
                PurchaseDate = purchase.PurchaseDate
            };

            foreach (var detail in purchase.PurchaseDetails)
            {
                var previouslyReturned = await _purchaseReturnRepository.GetPreviouslyReturnedQuantityAsync(detail.PurchaseDetailId);
                //var remainingReturnableQuantity = detail.Quantity - previouslyReturned;
                var remainingReturnableQuantity = Math.Max(0,detail.Quantity - previouslyReturned);


                var stock = await _stockRepository
                    .GetByProductBatchAndExpiryAsync(detail.ProductId,detail.BatchNumber,detail.ExpiryDate);
                var availableQuantity = Math.Min(remainingReturnableQuantity, stock?.Quantity ?? 0);
                dto.Items.Add(new PurchaseReturnItemDto
                {
                    PurchaseDetailId = detail.PurchaseDetailId,
                    ProductId = detail.ProductId,
                    ProductName = detail.Product.ProductName,
                    BatchNumber = detail.BatchNumber,
                    ExpiryDate = detail.ExpiryDate,
                    PurchasedQuantity = detail.Quantity,
                    PreviouslyReturnedQuantity = previouslyReturned,
                    AvailableReturnQuantity = availableQuantity,
                    PurchasePrice = detail.PurchasePrice,
                    GSTPercentage = detail.GSTPercentage,
                    StockId = stock?.StockId ?? 0,
                    AvailableStock = stock?.Quantity ?? 0
                });

            }

            return dto;
        }

        public async Task CreatePurchaseReturnAsync(CreatePurchaseReturnDto dto)
        {
            if (dto.PurchaseId <= 0)
                throw new Exception("Invalid purchase.");

            if (dto.Items == null || !dto.Items.Any())
                throw new Exception("Please select at least one item for return.");

            var purchase = await _purchaseRepository
                .GetByIdForReturnAsync(dto.PurchaseId);

            if (purchase == null)
                throw new Exception("Purchase not found.");

            if (purchase.Status != "Completed")
                throw new Exception("Only completed purchases can be returned.");
            if (dto.ReturnDate.Date < purchase.PurchaseDate.Date)
                throw new Exception("Return date cannot be earlier than purchase date.");

            if (dto.ReturnDate.Date > DateTime.Today)
                throw new Exception("Future return date is not allowed.");
            if (string.IsNullOrWhiteSpace(dto.Reason))
                throw new Exception("Please provide a purchase return reason.");

            if (dto.Reason.Length > 500)
                throw new Exception("Return reason cannot exceed 500 characters.");
            // Prevent duplicate purchase details
            if (dto.Items
                .GroupBy(x => x.PurchaseDetailId)
                .Any(g => g.Count() > 1))
            {
                throw new Exception("Duplicate purchase items are not allowed.");
            }
            if (dto.Items
               .GroupBy(x => x.StockId)
               .Any(g => g.Count() > 1))
            {
                throw new Exception("Duplicate stock entries are not allowed.");
            }

            // Validate combined quantity for the same stock
            foreach (var group in dto.Items.GroupBy(x => x.StockId))
            {
                var stock = await _stockRepository.GetByIdForReturnAsync(group.Key);

                var totalReturnQuantity = group.Sum(x => x.Quantity);

                if (stock == null || totalReturnQuantity > stock.Quantity)
                {
                    throw new Exception("Total return quantity exceeds available stock.");
                }
            }
            var returnItems = new List<(PurchaseDetail Detail, Stock Stock, int Quantity)>();

            foreach (var item in dto.Items)
            {
                if (item.Quantity <= 0)
                    throw new Exception("Return quantity must be greater than zero.");

                var detail = purchase.PurchaseDetails.FirstOrDefault(x => x.PurchaseDetailId == item.PurchaseDetailId);

                if (detail == null)
                    throw new Exception("Invalid purchase detail selected.");

                var previouslyReturned = await _purchaseReturnRepository
                    .GetPreviouslyReturnedQuantityAsync(detail.PurchaseDetailId);

                var remainingReturnableQuantity = detail.Quantity - previouslyReturned;

                if (item.Quantity > remainingReturnableQuantity)
                    throw new Exception($"Maximum returnable quantity for {detail.Product.ProductName} is {remainingReturnableQuantity}.");

                var stock = await _stockRepository.GetByIdForReturnAsync(item.StockId);

                if (stock == null || stock.ProductId != detail.ProductId || stock.BatchNumber != detail.BatchNumber || stock.ExpiryDate != detail.ExpiryDate ||
                    stock.Quantity < item.Quantity)
                {
                    throw new Exception(
                        $"Insufficient or invalid stock batch for {detail.Product.ProductName}.");
                }


                returnItems.Add((detail, stock, item.Quantity));
            }

            decimal totalAmount = 0;
            decimal totalGSTAmount = 0;

            foreach (var item in returnItems)
            {
                var itemAmount = item.Quantity * item.Detail.PurchasePrice;

                var gstAmount = Math.Round(
                    itemAmount * item.Detail.GSTPercentage / 100,
                    2);

                totalAmount += itemAmount;
                totalGSTAmount += gstAmount;
            }

            var netAmount = totalAmount + totalGSTAmount;
            try
            {
               
                await _unitOfWork.BeginTransactionAsync();
                var sequenceNumber = await _purchaseReturnRepository.GetNextReturnNumberAsync();
                var returnNumber = $"PR-{dto.ReturnDate.Year}-{sequenceNumber:D6}";
                var purchaseReturn = new PurchaseReturn(
                    returnNumber,
                    purchase.PurchaseId,
                    dto.ReturnDate,
                    dto.Reason,
                    totalAmount,
                    totalGSTAmount,
                    netAmount
                );
                foreach (var item in returnItems)
                {
                    var itemAmount = item.Quantity * item.Detail.PurchasePrice;

                    var gstAmount = Math.Round(
                        itemAmount * item.Detail.GSTPercentage / 100,
                        2);

                    var itemTotal = itemAmount + gstAmount;

                    var returnDetail = new PurchaseReturnDetail(
                        0, // Parent relationship will set PurchaseReturnId
                        item.Detail.PurchaseDetailId,
                        item.Detail.ProductId,
                        item.Stock.StockId,
                        item.Quantity,
                        item.Detail.PurchasePrice,
                        item.Detail.GSTPercentage,
                        gstAmount,
                        itemTotal
                    );

                    purchaseReturn.PurchaseReturnDetails.Add(returnDetail);
                    item.Stock.DecreaseQuantity(item.Quantity);
                    var ledger = new StockLedger(
                    item.Detail.ProductId,
                    item.Detail.BatchNumber ?? string.Empty,
                    item.Stock,
                   "PurchaseReturn",
                   -item.Quantity,
                   item.Stock.Quantity,
                   purchaseReturn.ReturnNumber,
                  "Stock returned to supplier"
    );

                    await _stockLedgerRepository.AddAsync(ledger);
                }
                await _purchaseReturnRepository.AddAsync(purchaseReturn);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await _unitOfWork.RollbackTransactionAsync();

                throw new InvalidOperationException(
                    "Stock was updated by another user. Please refresh the stock and try again.");
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
