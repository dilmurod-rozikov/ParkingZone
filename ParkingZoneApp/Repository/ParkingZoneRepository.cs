using ParkingZoneApp.Data;
using ParkingZoneApp.Models;
using ParkingZoneApp.Repository.Interfaces;

namespace ParkingZoneApp.Repository
{
    public class ParkingZoneRepository : Repository<ParkingZone>, IParkingZoneRepository<ParkingZone>
    {
        public ParkingZoneRepository(ApplicationDbContext context) : base(context)
        { }
    }
}
