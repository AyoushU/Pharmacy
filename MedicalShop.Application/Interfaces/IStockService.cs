using MedicalShop.Application.DTOs.Stock;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.Interfaces
{
    public interface IStockService
    {

        Task<List<StockDto>> GetAllStockListAsync();
    }
}
