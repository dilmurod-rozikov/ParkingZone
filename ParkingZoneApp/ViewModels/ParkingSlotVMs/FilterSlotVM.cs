using ParkingZoneApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingSlotVMs
{
    public class FilterSlotVM
    {
        [Required]
        public Guid ParkingZoneId { get; set; }

        public SlotCategory? Category { get; set; }

        public bool? IsSlotFree { get; set; }
    }
}
