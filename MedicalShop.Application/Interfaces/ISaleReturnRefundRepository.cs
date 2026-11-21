using MedicalShop.Domain.Entities;

namespace MedicalShop.Application.Interfaces
    {
        public interface ISaleReturnRefundRepository
        {
            Task AddAsync(SaleReturnRefund refund);
        }
    }

