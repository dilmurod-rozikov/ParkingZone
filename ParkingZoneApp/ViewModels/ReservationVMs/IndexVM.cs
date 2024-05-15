using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ReservationVMs
{
    public class IndexVM
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public uint Duration { get; set; }

        [Required]
        public string ZoneAddress { get; set; }

        [Required]
        public string ZoneName { get; set; }

        [Required]
        public string VehicleNumber { get; set; }

        [Required]
        public int SlotNumber { get; set; }

        public IndexVM() { }

        public IndexVM(Reservation reservation, ParkingSlot slot, ParkingZone zone)
        {
            StartDate = reservation.StartingTime;
            Duration = reservation.Duration;
            VehicleNumber = reservation.VehicleNumber;
            SlotNumber = slot.Number;
            ZoneAddress = zone.Address;
            ZoneName = zone.Name;
        }

        public static IEnumerable<IndexVM> MapToVM
            (IEnumerable<Reservation> reservations, IParkingZoneService _parkingZoneService, IParkingSlotService _parkingSlotService)
                => reservations.Select(reservation =>
            {
                var zone = _parkingZoneService.GetById(reservation.ParkingZoneId);
                var slot = _parkingSlotService.GetById(reservation.ParkingSlotId);

                return new IndexVM(reservation, slot, zone);
            }
            ).OrderByDescending(x => x.StartDate);
    }
}