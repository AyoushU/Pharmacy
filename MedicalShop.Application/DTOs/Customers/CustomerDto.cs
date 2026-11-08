using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.Customers
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }

        public string CustomerCode { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public decimal OpeningBalance { get; set; }

        public bool IsActive { get; set; }
    }
}
