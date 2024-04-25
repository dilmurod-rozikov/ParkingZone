using ParkingZoneApp.Models.Entities;

namespace ParkingZoneApp.Services.Interfaces
{
    public interface IParkingSlotService : IServices<ParkingSlot>
    {
        public ICollection<ParkingSlot> GetSlotsByZoneId(Guid parkingZoneId);

        public bool IsUniqueNumber(Guid id, int number);

    }
}
