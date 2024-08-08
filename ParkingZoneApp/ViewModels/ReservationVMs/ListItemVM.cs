using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ReservationVMs
{
    public class ListItemVM
    {
        [Required]
        public Guid Id { get; set; }

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

        public DateTime FinishDate
        {
            get => StartDate.AddHours(Duration);
        }

        public ListItemVM() { }

        public ListItemVM(Reservation reservation, ParkingSlot slot, ParkingZone zone)
        {
            Id = reservation.Id;
            StartDate = reservation.StartingTime;
            Duration = reservation.Duration;
            VehicleNumber = reservation.VehicleNumber;
            SlotNumber = slot.Number;
            ZoneAddress = zone.Address;
            ZoneName = zone.Name;
        }

        public static IEnumerable<ListItemVM> MapToVM(IEnumerable<Reservation> reservations, IEnumerable<ParkingZone> parkingZones, IEnumerable<ParkingSlot> parkingSlots)
        {
            var tasks = reservations.Select(reservation =>
            {
                var zone = parkingZones.FirstOrDefault(z => z.Id == reservation.ParkingZoneId);
                var slot = parkingSlots.FirstOrDefault(s => s.Id == reservation.ParkingSlotId);

                return new ListItemVM(reservation, slot, zone);
            });

            return tasks.OrderByDescending(x => x.StartDate);
        }
    }
}