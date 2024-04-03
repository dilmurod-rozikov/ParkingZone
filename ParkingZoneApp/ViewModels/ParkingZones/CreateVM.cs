using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class CreateVM
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public DateOnly CreatedDate { get; init; }

        public ParkingZone MapToModel(CreateVM parkingZone)
        {
            return new ParkingZone
            {
                Name = parkingZone.Name,
                Address = parkingZone.Address,
                CreatedDate = parkingZone.CreatedDate
            };
        }
    }
}
