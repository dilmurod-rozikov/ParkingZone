using Nest;
using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingZoneVMs
{
    public class CreateVM
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Name must be between 3 and 100 characters long.")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Name must be between 3 and 100 characters long.")]
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
