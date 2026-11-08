using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.DTOs.Categories
{

    public class CreateCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
