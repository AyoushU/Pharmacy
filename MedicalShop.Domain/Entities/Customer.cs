

namespace MedicalShop.Domain.Entities
{
   

    public class Customer
    {
        public int CustomerId { get; private set; }

        public string CustomerCode { get; private set; } = string.Empty;

        public string CustomerName { get; private set; } = string.Empty;

        public string? Phone { get; private set; }

        public string? Email { get; private set; }

        public string? Address { get; private set; }

        public decimal OpeningBalance { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime? ModifiedDate { get; private set; }

        private Customer()
        {
        }

        public Customer(
            string customerCode,
            string customerName,
            string? phone = null,
            string? email = null,
            string? address = null,
            decimal openingBalance = 0)
        {
            CustomerCode = customerCode;
            CustomerName = customerName;
            Phone = phone;
            Email = email;
            Address = address;
            OpeningBalance = openingBalance;

            IsActive = true;
            CreatedDate = DateTime.UtcNow;
        }

        public void Update(
            string customerCode,
            string customerName,
            string? phone,
            string? email,
            string? address,
            decimal openingBalance)
        {
            CustomerCode = customerCode;
            CustomerName = customerName;
            Phone = phone;
            Email = email;
            Address = address;
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
