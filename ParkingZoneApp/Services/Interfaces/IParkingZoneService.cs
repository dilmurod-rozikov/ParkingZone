using ParkingZoneApp.Models;

namespace ParkingZoneApp.Services.Interfaces
{
    public interface IParkingZoneService : IServices<ParkingZone>
    {
        public new void Insert(ParkingZone parkingZone);
    }
}
