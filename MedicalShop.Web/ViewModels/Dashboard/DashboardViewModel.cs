namespace MedicalShop.Web.ViewModels.Dashboard
{
        public class DashboardViewModel
        {
            public decimal TodaySales { get; set; }

            public int TotalProducts { get; set; }

            public int LowStockCount { get; set; }

            public int NearExpiryCount { get; set; }
        }
    }

