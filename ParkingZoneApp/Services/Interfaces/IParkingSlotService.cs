using ParkingZoneApp.Models.Entities;

namespace ParkingZoneApp.Services.Interfaces
{
    public interface IParkingSlotService : IServices<ParkingSlot>
    {
        ICollection<ParkingSlot> GetSlotsByZoneId(Guid zoneId);
    }
}
