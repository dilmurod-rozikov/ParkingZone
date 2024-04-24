using ParkingZoneApp.Data;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Repository.Interfaces;

namespace ParkingZoneApp.Repository
{
    public class ParkingSlotRepository : Repository<ParkingSlot>, IParkingSlotRepository
    {
        private readonly ApplicationDbContext _context;
        public ParkingSlotRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
