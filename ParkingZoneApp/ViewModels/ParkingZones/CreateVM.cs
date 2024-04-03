using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class CreateVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateOnly CreatedDate { get; init; }

        public ParkingZone MapToModel(CreateVM parkingZoneCreateVM)
        {
            return new ParkingZone
            {
                Id = parkingZoneCreateVM.Id,
                Name = parkingZoneCreateVM.Name,
                Address = parkingZoneCreateVM.Address,
                CreatedDate = parkingZoneCreateVM.CreatedDate
            };
        }
    }
}
