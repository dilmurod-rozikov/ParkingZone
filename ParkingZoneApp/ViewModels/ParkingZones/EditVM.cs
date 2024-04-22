using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class EditVM
    {
        [Required]
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public EditVM() { }

        public EditVM(ParkingZone parkingZone)
        {
            Id = parkingZone.Id;
            Name = parkingZone.Name;
            Address = parkingZone.Address;
        }

        public ParkingZone MapToModel(ParkingZone parkingZone)
        {
            parkingZone.Id = Id;
            parkingZone.Name = Name;
            parkingZone.Address = Address;
            return parkingZone;
        }
    }
}
