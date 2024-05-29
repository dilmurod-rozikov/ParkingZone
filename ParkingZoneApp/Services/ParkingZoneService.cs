using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services.Interfaces;

namespace ParkingZoneApp.Services
{
    public class ParkingZoneService : Services<ParkingZone>, IParkingZoneService
    {
        public ParkingZoneService(IParkingZoneRepository repository) : base(repository) { }

        public new void Insert(ParkingZone parkingZone)
        {
            parkingZone.Id = Guid.NewGuid();
            parkingZone.CreatedDate = DateOnly.FromDateTime(DateTime.Today);
            base.Insert(parkingZone);
        }

        public Dictionary<SlotCategory, long> FilterByPeriodOnSlotCategory(ParkingZone zone, PeriodRange range)
        {
             return zone.ParkingSlots
                .GroupBy(slot => slot.Category)
                .ToDictionary(category => category.Key, slots => slots
                .SelectMany(slot => slot.Reservations
                .Where(reservation => reservation.StartingTime > DateTime.Now.AddDays(0-range)
                        && reservation.StartingTime.AddHours(reservation.Duration) < DateTime.Now))
                .Sum(reservation => reservation.Duration));
        }
    }
}
