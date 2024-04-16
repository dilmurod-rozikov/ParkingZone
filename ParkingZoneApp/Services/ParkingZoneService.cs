using ParkingZoneApp.Models;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services.Interfaces;

namespace ParkingZoneApp.Services
{
    public class ParkingZoneService : Services<ParkingZone>, IParkingZoneService
    {
        public ParkingZoneService(IParkingZoneRepository repository) : base(repository)
        {
        }

        public override void Insert(ParkingZone parkingZone)
        {
            parkingZone.Id = Guid.NewGuid();
            parkingZone.CreatedDate = DateOnly.FromDateTime(DateTime.Today);
            base.Insert(parkingZone);
        }
    }
}
