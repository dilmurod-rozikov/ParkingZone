using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingZoneVMs
{
    public class EditVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Name must be between 3 and 100 characters long.")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Name must be between 3 and 100 characters long.")]
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
