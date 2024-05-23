using ParkingZoneApp.Enums;
using ParkingZoneApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingSlotVMs
{
    public class FilterSlotVM
    {
        [Required]
        public Guid ParkingZoneId { get; set; }

        public SlotCategory? Category { get; set; } = null;

        public bool? IsSlotFree { get; set; } = null;
    }
}
