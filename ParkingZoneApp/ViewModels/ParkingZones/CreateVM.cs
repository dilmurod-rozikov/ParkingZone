using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class CreateVM
    {
        [Required]
        public Guid? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateOnly? CreatedDate { get; init; }

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
