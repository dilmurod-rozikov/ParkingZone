using ParkingZoneApp.Data;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Repository.Interfaces;

namespace ParkingZoneApp.Repository
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        private readonly ApplicationDbContext _context;
        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
