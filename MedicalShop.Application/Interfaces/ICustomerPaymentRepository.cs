using MedicalShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.Interfaces
{
    public interface ICustomerPaymentRepository
    {
        Task AddAsync(CustomerPayment customerPayment);
    }
}
