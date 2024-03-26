using ParkingZoneApp.Data;
using ParkingZoneApp.IRepository;
using ParkingZoneApp.Models;

namespace ParkingZoneApp.Repository
{
    public class ParkingZoneRepository : Repository<ParkingZone>, IParkingZoneRepository
    {
        public ParkingZoneRepository(ApplicationDbContext context) : base(context)
        { }
    }
}
