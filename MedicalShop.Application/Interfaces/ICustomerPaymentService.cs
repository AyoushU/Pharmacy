using MedicalShop.Application.DTOs.CustomerPayment;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Application.Interfaces
{
    public interface ICustomerPaymentService
    {
        Task ReceivePaymentAsync(ReceiveCustomerPaymentDto dto);
    }
}
