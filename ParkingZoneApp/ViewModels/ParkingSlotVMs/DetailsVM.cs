using ParkingZoneApp.Enums;
using ParkingZoneApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingSlotVMs
{
    public class DetailsVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public SlotCategory Category { get; set; }

        [Required]
        public Guid ParkingZoneId { get; set; }

        public DetailsVM() { }

        public DetailsVM(ParkingSlot parkingSlot)
        {
            Id = parkingSlot.Id;
            Number = parkingSlot.Number;
            Category = parkingSlot.Category;
            IsAvailable = parkingSlot.IsAvailable;
            ParkingZoneId = parkingSlot.ParkingZoneId;
        }

        public ParkingSlot MapToModel()
        {
            return new ParkingSlot
            {
                Id = Id,
                Number = Number,
                Category = Category,
                IsAvailable = IsAvailable,
                ParkingZoneId = ParkingZoneId
            };
        }
    }
}
