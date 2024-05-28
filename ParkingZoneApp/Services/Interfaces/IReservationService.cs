using ParkingZoneApp.Models.Entities;

namespace ParkingZoneApp.Services.Interfaces
{
    public interface IReservationService : IServices<Reservation>
    {
        public IEnumerable<Reservation> GetReservationsByUserId(string userId);

        public IEnumerable<Reservation> GetReservationsByZoneId(Guid zoneId);
    }
}
