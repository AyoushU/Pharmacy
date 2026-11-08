using MedicalShop.Application.DTOs.Purchases;
using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;


namespace MedicalShop.Application.Services
{

    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IInventoryService _inventoryService;
        private readonly IUnitOfWork _unitOfWork;
        public PurchaseService(IPurchaseRepository purchaseRepository, IInventoryService inventoryService, IUnitOfWork unitOfWork)
        {
            _purchaseRepository = purchaseRepository;
            _inventoryService = inventoryService;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PurchaseDto>> GetAllAsync()
        {
            var purchases = await _purchaseRepository.GetAllAsync();

            return purchases.Select(x => new PurchaseDto
            {
                PurchaseId = x.PurchaseId,
                PurchaseNumber = x.PurchaseNumber,
                SupplierId = x.SupplierId,
                SupplierName = x.Supplier.SupplierName,
                PurchaseDate = x.PurchaseDate,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceDate = x.InvoiceDate,
                TotalAmount = x.TotalAmount,
                GSTAmount = x.GSTAmount,
                NetAmount = x.NetAmount,
                Status = x.Status,

                Details = x.PurchaseDetails.Select(d => new PurchaseDetailDto
                {
                    PurchaseDetailId = d.PurchaseDetailId,
                    ProductId = d.ProductId,
                    ProductName = d.Product.ProductName,
                    BatchNumber = d.BatchNumber,
                    ExpiryDate = d.ExpiryDate,
                    Quantity = d.Quantity,
                    PurchasePrice = d.PurchasePrice,
                    GSTPercentage = d.GSTPercentage,
                    TotalAmount = d.TotalAmount
                }).ToList()
            }).ToList();
        }

        public async Task<PurchaseDto?> GetByIdAsync(int id)
        {
            var purchase = await _purchaseRepository.GetByIdAsync(id);

            if (purchase == null)
                return null;

            return new PurchaseDto
            {
                PurchaseId = purchase.PurchaseId,
                PurchaseNumber = purchase.PurchaseNumber,
                SupplierId = purchase.SupplierId,
                SupplierName = purchase.Supplier.SupplierName,
                PurchaseDate = purchase.PurchaseDate,
                InvoiceNumber = purchase.InvoiceNumber,
                InvoiceDate = purchase.InvoiceDate,
                TotalAmount = purchase.TotalAmount,
                GSTAmount = purchase.GSTAmount,
                NetAmount = purchase.NetAmount,
                Status = purchase.Status,

                Details = purchase.PurchaseDetails.Select(d => new PurchaseDetailDto
                {
                    PurchaseDetailId = d.PurchaseDetailId,
                    ProductId = d.ProductId,
                    ProductName = d.Product.ProductName,
                    BatchNumber = d.BatchNumber,
                    ExpiryDate = d.ExpiryDate,
                    Quantity = d.Quantity,
                    PurchasePrice = d.PurchasePrice,
                    GSTPercentage = d.GSTPercentage,
                    TotalAmount = d.TotalAmount
                }).ToList()
            };
        }

        public async Task<int> CreateAsync(CreatePurchaseDto dto)
        {
            if (dto.Details == null || dto.Details.Count == 0)
                throw new InvalidOperationException(
                    "At least one product is required for purchase.");

            decimal totalAmount = 0;
            decimal gstAmount = 0;

            var purchaseNumber =
                $"PUR-{DateTime.UtcNow:yyyyMMddHHmmssfff}";

            var purchase = new Purchase(
                purchaseNumber,
                dto.SupplierId,
                dto.PurchaseDate,
                dto.InvoiceNumber,
                dto.InvoiceDate,
                0,
                0,
                0,
                "Draft");

            foreach (var detail in dto.Details)
            {
                if (detail.ProductId <= 0)
                    throw new InvalidOperationException("Please select a product.");

                if (detail.Quantity <= 0)
                    throw new InvalidOperationException(
                        "Quantity must be greater than zero.");

                if (detail.PurchasePrice < 0)
                    throw new InvalidOperationException(
                        "Purchase price cannot be negative.");

                if (detail.GSTPercentage < 0 ||
                    detail.GSTPercentage > 100)
                    throw new InvalidOperationException(
                        "GST percentage must be between 0 and 100.");

                var baseAmount =
                    detail.Quantity * detail.PurchasePrice;

                var detailGST =
                    baseAmount * detail.GSTPercentage / 100;

                var detailTotal =
                    baseAmount + detailGST;

                totalAmount += baseAmount;
                gstAmount += detailGST;

                var purchaseDetail = new PurchaseDetail(
                    0,
                    detail.ProductId,
                    detail.BatchNumber,
                    detail.ExpiryDate,
                    detail.Quantity,
                    detail.PurchasePrice,
                    detail.GSTPercentage,
                    detailTotal);

                purchase.PurchaseDetails.Add(purchaseDetail);
            }

            var netAmount = totalAmount + gstAmount;

            purchase.Update(
                dto.SupplierId,
                dto.PurchaseDate,
                dto.InvoiceNumber,
                dto.InvoiceDate,
                totalAmount,
                gstAmount,
                netAmount,
                "Draft");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1. Purchase + PurchaseDetails
                await _purchaseRepository.AddAsync(purchase);

                // 2. Stock + StockLedger
                foreach (var detail in dto.Details)
                {
                    await _inventoryService.AddStockAsync(
                        detail.ProductId,
                        detail.BatchNumber,
                        detail.ExpiryDate,
                        detail.Quantity,
                        detail.PurchasePrice,

                        0, // SellingPrice - abhi temporary
                        "Purchase",
                        purchaseNumber,
                        $"Stock added from purchase {purchaseNumber}");

                }

                // 3. Purchase + Stock + Ledger ek saath save
                await _unitOfWork.SaveChangesAsync();

                // 4. Commit
                await _unitOfWork.CommitTransactionAsync();

                return purchase.PurchaseId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
