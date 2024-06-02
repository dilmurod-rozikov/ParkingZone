using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;

namespace ParkingZoneApp.Services
{
    public class ParkingSlotService : Services<ParkingSlot>, IParkingSlotService
    {
        private readonly IParkingSlotRepository _parkingSlotRepository;

        public ParkingSlotService(IParkingSlotRepository parkingSlotRepository) : base(parkingSlotRepository)
        {
            _parkingSlotRepository = parkingSlotRepository;
        }

        public async Task<ICollection<ParkingSlot>> FilterAsync(FilterSlotVM filterSlotVM)
        {
            var query = await GetSlotsByZoneIdAsync(filterSlotVM.ParkingZoneId);

            if (filterSlotVM.Category.HasValue)
                query = query.Where(x => x.Category == filterSlotVM.Category.Value).ToList();

            if (filterSlotVM.IsSlotFree.HasValue)
                query = query.Where(x => !x.IsInUse == filterSlotVM.IsSlotFree.Value).ToList();
            
            return query;
        }

        public async Task<bool> IsUniqueNumberAsync(Guid zoneId, int number)
        {
            var slots = await GetSlotsByZoneIdAsync(zoneId);
            return slots.Any(x => x.Number == number);
        }

        public async Task<ICollection<ParkingSlot>> GetSlotsByZoneIdAsync(Guid parkingZoneId)
        {
            var slots = await _parkingSlotRepository.GetAll();
            return slots.Where(x => x.ParkingZoneId == parkingZoneId).ToList();
        }

        public bool IsSlotFreeForReservation(ParkingSlot slot, DateTime startTime, uint duration)
        {
            var reservations = slot.Reservations.ToList();
            return !reservations.Any(x =>
                (startTime >= x.StartingTime & startTime.AddHours(duration) <= x.StartingTime.AddHours(x.Duration)) |
                (startTime >= x.StartingTime & startTime < x.StartingTime.AddHours(x.Duration)) |
                (startTime <= x.StartingTime & x.StartingTime < startTime.AddHours(duration))
            );
        }

        public async Task<IEnumerable<ParkingSlot>> GetAllFreeSlotsAsync(Guid zoneId, DateTime startingTime, uint duration)
        {
            var slots = await GetSlotsByZoneIdAsync(zoneId);
            return slots.Where(x => x.IsAvailable && IsSlotFreeForReservation(x, startingTime, duration));
        }

        public async new Task Insert(ParkingSlot parkingSlot)
        {
            parkingSlot.Id = Guid.NewGuid();
            await base.Insert(parkingSlot);
        }
    }
}
