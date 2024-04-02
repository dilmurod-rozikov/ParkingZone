using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class DeleteVM
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateOnly? CreatedDate { get; init; }

        public DeleteVM MapToModel(ParkingZone parkingZone)
        {
            return new DeleteVM
            {
                Id = parkingZone.Id,
                Name = parkingZone.Name,
                Address = parkingZone.Address,
                CreatedDate = parkingZone.CreatedDate,
            };
        }
    }
}
