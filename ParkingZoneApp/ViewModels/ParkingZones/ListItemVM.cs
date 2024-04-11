using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class ListItemVM
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateOnly CreatedDate { get; init; }

        public ListItemVM()
        {
        }

        public ListItemVM(ParkingZone parkingZone)
        {
            Id = parkingZone.Id;
            Name = parkingZone.Name;
            Address = parkingZone.Address;
            CreatedDate = parkingZone.CreatedDate;
        }


        public static IEnumerable<ListItemVM> MapToVM(IEnumerable<ParkingZone> parkingZones)
        {
            return parkingZones.Select(parkingZone => new ListItemVM(parkingZone));
        }
    }
}
