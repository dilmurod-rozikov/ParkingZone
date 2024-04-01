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

        public override void Insert(ParkingZone parkingZone)
        {
            parkingZone.Id = Guid.NewGuid();
            base.Insert(parkingZone);
        }
    }
}
