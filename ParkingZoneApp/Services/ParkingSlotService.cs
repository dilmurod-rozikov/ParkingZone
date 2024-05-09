using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services.Interfaces;

namespace ParkingZoneApp.Services
{
    public class ParkingSlotService : Services<ParkingSlot>, IParkingSlotService
    {
        private readonly IParkingSlotRepository _parkingSlotRepository;

        public ParkingSlotService(IParkingSlotRepository parkingSlotRepository) : base(parkingSlotRepository)
        {
            _parkingSlotRepository = parkingSlotRepository;
        }

        public bool IsUniqueNumber(Guid zoneId, int number)
        {
            return GetSlotsByZoneId(zoneId).Any(x => x.Number == number);
        }

        public ICollection<ParkingSlot> GetSlotsByZoneId(Guid parkingZoneId)
        {
            return _parkingSlotRepository.GetAll().Where(x => x.ParkingZoneId == parkingZoneId).ToList();
        }

        public bool IsSlotFreeForReservation(ParkingSlot slot, DateTime startTime, int duration)
        {
            return !slot.Reservations.Any(x => 
                (startTime >= x.StartingTime && startTime.AddHours(duration) <= x.StartingTime.AddHours(x.Duration)) ||
                (startTime >= x.StartingTime && startTime < x.StartingTime.AddHours(x.Duration)) ||
                (startTime <= x.StartingTime && x.StartingTime < startTime.AddHours(duration))

            );
        }

        public IEnumerable<ParkingSlot> GetAllFreeSlots(Guid zoneId, DateTime startingTime, int duration)
        {
            return GetSlotsByZoneId(zoneId).Where(x => x.IsAvailable && IsSlotFreeForReservation(x, startingTime, duration));
        }

        public new void Insert(ParkingSlot parkingSlot)
        {
            parkingSlot.Id = Guid.NewGuid();
            base.Insert(parkingSlot);
        }
    }
}
