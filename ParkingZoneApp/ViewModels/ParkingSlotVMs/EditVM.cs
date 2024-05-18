using ParkingZoneApp.Enums;
using ParkingZoneApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingSlotVMs
{
    public class EditVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public SlotCategory Category { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public Guid ParkingZoneId { get; set; }

        [Required]
        public bool IsSlotInUse { get; set; }

        public EditVM() { }

        public EditVM(ParkingSlot parkingSlot)
        {
            Id = parkingSlot.Id;
            Number = parkingSlot.Number;
            Category = parkingSlot.Category;
            IsAvailable = parkingSlot.IsAvailable;
            ParkingZoneId = parkingSlot.ParkingZoneId;
            IsSlotInUse = parkingSlot.IsSlotInUse;
        }

        public ParkingSlot MapToModel(ParkingSlot parkingSlot)
        {
            parkingSlot.Number = Number;
            parkingSlot.Category = Category;
            parkingSlot.IsAvailable = IsAvailable;
            parkingSlot.ParkingZoneId = ParkingZoneId;
            return parkingSlot;
        }
    }
}
