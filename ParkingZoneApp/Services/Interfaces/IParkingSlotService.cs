using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;

namespace ParkingZoneApp.Services.Interfaces
{
    public interface IParkingSlotService : IServices<ParkingSlot>
    {
        public Task<ICollection<ParkingSlot>> FilterAsync(FilterSlotVM filterSlotVM);

        public Task<ICollection<ParkingSlot>> GetSlotsByZoneIdAsync(Guid parkingZoneId);

        public Task<bool> IsUniqueNumberAsync(Guid zoneId, int number);

        public Task<IEnumerable<ParkingSlot>> GetAllFreeSlotsAsync(Guid zoneId, DateTime startingTime, uint duration);

        public bool IsSlotFreeForReservation(ParkingSlot slot, DateTime startTime, uint duration);

        public new Task Insert(ParkingSlot parkingSlot);
    }
}
