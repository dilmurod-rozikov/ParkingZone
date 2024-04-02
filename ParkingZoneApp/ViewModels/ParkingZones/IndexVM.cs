using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class IndexVM
    {
        public IEnumerable<ParkingZone> ParkingZones { get; set; }

        public IndexVM MapToModel(IEnumerable<ParkingZone> parkingZone)
        {
            return new IndexVM
            {
                ParkingZones = parkingZone
            };
        }
    }
}
