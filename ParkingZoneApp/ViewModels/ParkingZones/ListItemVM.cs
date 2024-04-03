using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class ListItemVM
    {
        public IEnumerable<ParkingZone> ParkingZones { get; set; }

        public ListItemVM MapToModel(IEnumerable<ParkingZone> parkingZone)
        {
            return new ListItemVM
            {
                ParkingZones = parkingZone
            };
        }
    }
}
