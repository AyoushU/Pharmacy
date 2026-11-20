using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.PurchaseReturn
{
    public class CreatePurchaseReturnItemDto
    {
        public int PurchaseDetailId { get; set; }

        public int StockId { get; set; }

        public int Quantity { get; set; }
    }
}
