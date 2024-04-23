using ParkingZoneApp.Models.Entities;

namespace ParkingZoneApp.Repository.Interfaces
{
    public interface IParkingSlotRepository : IRepository<ParkingSlot>
    {
        ICollection<ParkingSlot> GetAllParkingSlots();
    }
}
