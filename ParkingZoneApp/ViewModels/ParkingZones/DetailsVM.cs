using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class DetailsVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateOnly CreatedDate { get; init; }

        public DetailsVM(ParkingZone parkingZone)
        {
            Id = parkingZone.Id;
            Name = parkingZone.Name;
            Address = parkingZone.Address;
            CreatedDate = parkingZone.CreatedDate;
        }
    }
}
