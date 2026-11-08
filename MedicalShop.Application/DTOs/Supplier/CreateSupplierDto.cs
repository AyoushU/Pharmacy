using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.Supplier
{

    public class CreateSupplierDto
    {
        public string SupplierCode { get; set; } = string.Empty;

        public string SupplierName { get; set; } = string.Empty;

        public string? ContactPerson { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? GSTNumber { get; set; }

        public decimal OpeningBalance { get; set; }
    }
}
