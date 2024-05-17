using ParkingZoneApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ReservationVMs
{
    public class ProlongVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Range(1, uint.MaxValue, ErrorMessage = "Prolonging time must be at least 1 hour")]
        public uint ProlongDuration { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime FinishTime { get; set; }

        public Reservation MapToModel(Reservation reservation)
        {
            reservation.Duration += ProlongDuration;
            return reservation;
        }
    }
}
