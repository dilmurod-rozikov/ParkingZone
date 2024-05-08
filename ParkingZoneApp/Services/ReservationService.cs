using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services.Interfaces;

namespace ParkingZoneApp.Services
{
    public class ReservationService : Services<Reservation>, IReservationService
    {
        private readonly IParkingSlotRepository _parkingSlotRepository;
        public ReservationService(IReservationRepository reservationRepository, IParkingSlotRepository parkingSlotRepository)
            : base(reservationRepository)
        {
            _parkingSlotRepository = parkingSlotRepository;
        }
    }
}
