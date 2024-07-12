using ParkingZoneApp.Models.Entities;

namespace ParkingZoneApp.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> StorePaymentDetails(Payment payment);
    }
}
