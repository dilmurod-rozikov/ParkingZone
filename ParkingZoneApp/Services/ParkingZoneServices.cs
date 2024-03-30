using ParkingZoneApp.Models;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services.Interfaces;

namespace ParkingZoneApp.Services
{
    public class ParkingZoneServices : Services<ParkingZone>, IParkingZoneServices
    {
        public ParkingZoneServices(IRepository<ParkingZone> repository) : base(repository)
        {
        }
    }
}
