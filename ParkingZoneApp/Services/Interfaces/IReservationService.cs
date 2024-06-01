using ParkingZoneApp.Models.Entities;

namespace ParkingZoneApp.Services.Interfaces
{
    public interface IReservationService : IServices<Reservation>
    {
        public Task<IEnumerable<Reservation>> GetReservationsByUserId(string userId);

        public Task<IEnumerable<Reservation>> GetReservationsByZoneId(Guid zoneId);
    }
}
