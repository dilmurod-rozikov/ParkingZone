using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingSlots
{
    public class ListItemVM
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
        public ParkingZone ParkingZone { get; set; }

        public bool IsSlotFree {  get; set; }

        public ListItemVM() { }

        public ListItemVM(ParkingSlot slot)
        {
            Id = slot.Id;
            Number = slot.Number;
            Category = slot.Category;
            IsAvailable = slot.IsAvailable;
            ParkingZone = slot.ParkingZone;
            IsSlotFree = !slot.IsInUse;
        }

        public static IEnumerable<ListItemVM> MapToVM(IEnumerable<ParkingSlot> slots)
        {
            return slots.Select(slot => new ListItemVM(slot));
        }
    }
}
