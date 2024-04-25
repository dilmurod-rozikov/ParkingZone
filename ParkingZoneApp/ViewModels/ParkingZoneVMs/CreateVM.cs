using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingZoneVMs
{
    public class CreateVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public ParkingZone MapToModel()
        {
            return new ParkingZone
            {
                Name = Name,
                Address = Address,
            };
        }
    }
}
