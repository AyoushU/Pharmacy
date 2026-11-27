using MedicalShop.Application.DTOs.CustomerPayment;
using MedicalShop.Application.Interfaces;
using MedicalShop.Domain.Entities;

namespace MedicalShop.Application.Services
{
    public class CustomerPaymentService : ICustomerPaymentService
    {
        private readonly ICustomerPaymentRepository _customerPaymentRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerPaymentService(
            ICustomerPaymentRepository customerPaymentRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _customerPaymentRepository = customerPaymentRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ReceivePaymentAsync(ReceiveCustomerPaymentDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);

            if (customer == null)
                throw new Exception("Customer not found.");

            var payment = new CustomerPayment(
                dto.CustomerId,
                dto.PaymentDate,
                dto.Amount,
                dto.PaymentMethod,
                dto.ReferenceNumber,
                dto.Remarks
            );

            await _customerPaymentRepository.AddAsync(payment);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
