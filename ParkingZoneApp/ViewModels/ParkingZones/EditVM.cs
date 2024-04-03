using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class EditVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateOnly CreatedDate { get; set; }

        public EditVM MapToModel(ParkingZone parkingZone)
        {
            return new EditVM
            {
                Id = parkingZone.Id,
                Name = parkingZone.Name,
                Address = parkingZone.Address,
                CreatedDate = parkingZone.CreatedDate,
            };
        }
    }
}
