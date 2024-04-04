using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class CreateVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateOnly CreatedDate { get; init; }

        public ParkingZone MapToModel()
        {
            return new ParkingZone
            {
                Id = Id,
                Name = Name,
                Address = Address,
                CreatedDate = CreatedDate
            };
        }
    }
}
