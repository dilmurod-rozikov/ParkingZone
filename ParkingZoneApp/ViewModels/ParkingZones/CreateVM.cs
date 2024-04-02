using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class CreateVM
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateOnly? CreatedDate { get; init; }

        public CreateVM MapToModel(ParkingZone parkingZone)
        {
            return new CreateVM
            {
                Name = parkingZone.Name,
                Address = parkingZone.Address,
                CreatedDate = parkingZone.CreatedDate
            };
        }
    }
}
