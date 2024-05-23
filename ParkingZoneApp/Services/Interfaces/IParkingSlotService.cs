using ParkingZoneApp.Enums;
using ParkingZoneApp.Models.Entities;

namespace ParkingZoneApp.Services.Interfaces
{
    public interface IParkingSlotService : IServices<ParkingSlot>
    {
        public ICollection<ParkingSlot> FilterByFreeSlot(ICollection<ParkingSlot> query, bool? isSlotFree);

        public ICollection<ParkingSlot> FilterByCategory(ICollection<ParkingSlot> query, SlotCategory? category);

        public ICollection<ParkingSlot> GetSlotsByZoneId(Guid parkingZoneId);

        public bool IsUniqueNumber(Guid id, int number);

        public IEnumerable<ParkingSlot> GetAllFreeSlots(Guid zoneId, DateTime startingTime, uint duration);

        public bool IsSlotFreeForReservation(ParkingSlot slot, DateTime startTime, uint duration);

        public new void Insert(ParkingSlot parkingSlot);
    }
}
