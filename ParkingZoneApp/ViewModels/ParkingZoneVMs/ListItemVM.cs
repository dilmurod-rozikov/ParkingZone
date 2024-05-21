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

        public int NumberOfSlots { get; init; }

        public int SlotInUse { get; set; }

        public ListItemVM() { }

        public ListItemVM(ParkingZone parkingZone)
        {
            Id = parkingZone.Id;
            Name = parkingZone.Name;
            Address = parkingZone.Address;
            CreatedDate = parkingZone.CreatedDate;
            NumberOfSlots = parkingZone.ParkingSlots.Count;
            SlotInUse += parkingZone.ParkingSlots.Count(x => x.IsInUse);
        }


        public static IEnumerable<ListItemVM> MapToVM(IEnumerable<ParkingZone> parkingZones)
        {
            return parkingZones.Select(parkingZone => new ListItemVM(parkingZone));
        }
    }
}
