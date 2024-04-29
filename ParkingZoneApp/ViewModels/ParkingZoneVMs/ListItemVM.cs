using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingZoneVMs
{
    public class ListItemVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
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
