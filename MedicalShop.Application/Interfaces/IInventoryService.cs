using MedicalShop.Application.DTOs.Inventory;
using MedicalShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.Interfaces
{

    public interface IInventoryService
    {

        Task<List<InventoryDto>> GetAllStockAsync();
        Task AddStockAsync(
                int productId,
                string? batchNumber,
                DateTime? expiryDate,
                int quantity,
                decimal purchasePrice,
                decimal sellingPrice,
                string transactionType,
                string? referenceNumber = null,
                string? remarks = null);

            Task RemoveStockAsync(
                int productId,
                string? batchNumber,
                int quantity,
                string transactionType,
                string? referenceNumber = null,
                string? remarks = null);
        }

    }

