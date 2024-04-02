using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class DetailsVM
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateOnly? CreatedDate { get; set; }

        public DetailsVM MapToModel(ParkingZone parkingZone)
        {
            return new DetailsVM()
            {
                Id = parkingZone.Id,
                Name = parkingZone.Name,
                Address = parkingZone.Address,
                CreatedDate = parkingZone.CreatedDate,
            };
        }
    }
}
