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

        public ICollection<ParkingSlot> GetSlotsByZoneId(Guid zoneId)
        {
            return _parkingSlotRepository.GetAllParkingSlots().Where(x => x.ParkingZoneId == zoneId).ToList();
        }
    }
}
