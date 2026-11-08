using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Domain.Entities
{

    public class Category
    {
        public int CategoryId { get; private set; }

        public string CategoryName { get; private set; } = string.Empty;

        public string? Description { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public ICollection<Product> Products { get; private set; } // 
            = new List<Product>();

        private Category()
        {
        }

        public Category(
            string categoryName,
            string? description = null)
        {
            CategoryName = categoryName;
            Description = description;
            IsActive = true;
            CreatedDate = DateTime.UtcNow;
        }

        public void Update(
            string categoryName,
            string? description)
        {
            CategoryName = categoryName;
            Description = description;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
