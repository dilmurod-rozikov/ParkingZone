using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services.Interfaces;

namespace ParkingZoneApp.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public Task<bool> StorePaymentDetails(Payment payment)
        {
            payment.Id = Guid.NewGuid();
            payment.PaymentDate = DateTime.Now;
            return _paymentRepository.StorePaymentDetails(payment);
        }
    }
}
