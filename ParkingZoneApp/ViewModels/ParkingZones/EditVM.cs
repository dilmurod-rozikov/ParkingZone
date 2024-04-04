using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class EditVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateOnly CreatedDate { get; init; }

        public EditVM() {  }
        public EditVM(ParkingZone parkingZone)
        {
            Id = parkingZone.Id;
            Name = parkingZone.Name;
            Address = parkingZone.Address;
            CreatedDate = parkingZone.CreatedDate;
        }

        public ParkingZone MapToModel(ParkingZone parkingZone)
        {
            parkingZone.Id = Id;
            parkingZone.Name = Name;
            parkingZone.Address = Address;
            //parkingZone.CreatedDate = CreatedDate;
            return parkingZone;
        }
    }
}
