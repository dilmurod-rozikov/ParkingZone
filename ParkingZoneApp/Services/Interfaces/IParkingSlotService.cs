using ParkingZoneApp.Models.Entities;

namespace ParkingZoneApp.Services.Interfaces
{
    public interface IParkingSlotService : IServices<ParkingSlot>
    {
        public ICollection<ParkingSlot> GetSlotsByZoneId(Guid parkingZoneId);

        public bool IsUniqueNumber(Guid id, int number);

        public IEnumerable<ParkingSlot> GetAllFreeSlots(Guid zoneId, DateTime startingTime, int duration);

        public bool IsSlotFreeForReservation(ParkingSlot slot, DateTime startTime, int duration);

        public new void Insert(ParkingSlot parkingSlot);
    }
}
