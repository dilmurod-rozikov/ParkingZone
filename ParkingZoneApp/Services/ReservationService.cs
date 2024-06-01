using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services.Interfaces;

namespace ParkingZoneApp.Services
{
    public class ReservationService : Services<Reservation>, IReservationService
    {
        public ReservationService(IReservationRepository reservationRepository)
            : base(reservationRepository) { }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserId(string userId)
        {
            var reservations = await GetAll();
            return reservations.Where(x => x.UserId == userId);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByZoneId(Guid zoneId)
        {
            var reservations = await GetAll();
            return reservations.Where(x => x.ParkingZoneId == zoneId & x.IsActive);
        }
    }
}
