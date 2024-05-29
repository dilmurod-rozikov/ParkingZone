using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;

namespace ParkingZoneApp.Services.Interfaces
{
    public interface IParkingZoneService : IServices<ParkingZone>
    {
        public new void Insert(ParkingZone parkingZone);

        public Dictionary<SlotCategory, long> FilterByPeriodOnSlotCategory(ParkingZone zone, PeriodRange range);
    }
}
