using MedicalShop.Application.DTOs.PurchaseReturn;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.Interfaces
{
    public interface IPurchaseReturnService
    {
        Task<PurchaseReturnDto?> GetPurchaseForReturnAsync(int purchaseId);
        Task CreatePurchaseReturnAsync(CreatePurchaseReturnDto dto);
    }
}
