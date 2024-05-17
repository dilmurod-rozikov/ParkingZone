using ParkingZoneApp.Models.Entities;

namespace ParkingZoneApp.Services.Interfaces
{
    public interface IReservationService : IServices<Reservation>
    {
        public IEnumerable<Reservation> GetReservationsByUser(string userId);
    }
}
