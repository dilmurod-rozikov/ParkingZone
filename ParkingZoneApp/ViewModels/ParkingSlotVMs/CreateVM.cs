using ParkingZoneApp.Enums;
using ParkingZoneApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingSlotVMs
{
    public class CreateVM
    {
        [Required]
        public int Number { get; set; }

        [Required]
        public SlotCategory Category { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public Guid ParkingZoneId { get; set; }

        public ParkingSlot MapToModel()
        {
            return new ParkingSlot
            {
                Number = Number,
                Category = Category,
                IsAvailable = IsAvailable,
                ParkingZoneId = ParkingZoneId,
            };
        }
    }
}
