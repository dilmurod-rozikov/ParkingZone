using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services.Interfaces;

namespace ParkingZoneApp.Services
{
    public class ReservationService : Services<Reservation>, IReservationService
    {
        public ReservationService(IReservationRepository reservationRepository)
            : base(reservationRepository) { }

        public IEnumerable<Reservation> GetReservationsByUserId(string userId)
        {
            return GetAll().Where(x => x.UserId == userId);
        }

        public IEnumerable<Reservation> GetReservationsByZoneId(Guid zoneId)
        {
            return GetAll().Where(x => x.ParkingZoneId == zoneId & x.IsActive);
        }
    }
}
