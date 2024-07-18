using ParkingZoneApp.Models.Entities;

namespace ParkingZoneApp.Repository.Interfaces
{
    public interface IPaymentRepository
    {
        Task<bool> StorePaymentDetails(Payment payment);
    }
}
