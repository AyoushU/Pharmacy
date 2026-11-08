using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Domain.Entities
{

    public class Supplier
    {
        public int SupplierId { get; private set; }

        public string SupplierCode { get; private set; } = string.Empty;

        public string SupplierName { get; private set; } = string.Empty;

        public string? ContactPerson { get; private set; }

        public string? Phone { get; private set; }

        public string? Email { get; private set; }

        public string? Address { get; private set; }

        public string? GSTNumber { get; private set; }

        public decimal OpeningBalance { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime? ModifiedDate { get; private set; }

        private Supplier()
        {
        }

        public Supplier(
            string supplierCode,
            string supplierName,
            string? contactPerson = null,
            string? phone = null,
            string? email = null,
            string? address = null,
            string? gstNumber = null,
            decimal openingBalance = 0)
        {
            SupplierCode = supplierCode;
            SupplierName = supplierName;
            ContactPerson = contactPerson;
            Phone = phone;
            Email = email;
            Address = address;
            GSTNumber = gstNumber;
            OpeningBalance = openingBalance;

            IsActive = true;
            CreatedDate = DateTime.UtcNow;
        }

        public void Update(
            string supplierCode,
            string supplierName,
            string? contactPerson,
            string? phone,
            string? email,
            string? address,
            string? gstNumber,
            decimal openingBalance)
        {
            SupplierCode = supplierCode;
            SupplierName = supplierName;
            ContactPerson = contactPerson;
            Phone = phone;
            Email = email;
            Address = address;
            GSTNumber = gstNumber;
            OpeningBalance = openingBalance;

            ModifiedDate = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            ModifiedDate = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            ModifiedDate = DateTime.UtcNow;
        }
    }
}
