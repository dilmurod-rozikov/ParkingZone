using ParkingZoneApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingZoneVMs
{
    public class GetCurrentCarsVM
    {
        [Required]
        public string VehicleNumber { get; set; }

        [Required]
        public int Number { get; set; }

        public GetCurrentCarsVM() { }

        public GetCurrentCarsVM(Reservation reservation)
        {
            VehicleNumber = reservation.VehicleNumber;
            Number = reservation.ParkingSlot.Number;
        }
    }
}
