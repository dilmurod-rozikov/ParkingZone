using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class EditVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateOnly CreatedDate { get; set; }

        public ParkingZone MapToModel(EditVM parkingZoneEditVM)
        {
            return new ParkingZone
            {
                Id = parkingZoneEditVM.Id,
                Name = parkingZoneEditVM.Name,
                Address = parkingZoneEditVM.Address,
                CreatedDate = parkingZoneEditVM.CreatedDate,
            };
        }
    }
}
