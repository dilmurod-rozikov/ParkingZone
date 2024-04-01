using ParkingZoneApp.Models;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services.Interfaces;

namespace ParkingZoneApp.Services
{
    public class ParkingZoneServices : Services<ParkingZone>, IParkingZoneServices
    {
        public ParkingZoneServices(IParkingZoneRepository repository) : base(repository)
        {
        }
    }
}
