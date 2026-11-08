using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.Dashboard
{
    public class DashboardDto
    {
        public decimal TodaySales { get; set; }
        public int TotalProducts { get; set; }
        public int LowStockCount { get; set; }
        public int NearExpiryCount { get; set; }
    }
}
