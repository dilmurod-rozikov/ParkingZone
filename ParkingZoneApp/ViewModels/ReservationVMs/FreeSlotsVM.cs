using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ReservationVMs
{
    public class FreeSlotsVM
    {
        private static readonly DateTime now = DateTime.Now;

        [Required]
        public Guid Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be higher than 0")]
        public uint Duration { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid DateTime format")]
        public DateTime StartingTime { get; set; } = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

        [Required]
        public Guid SelectedZoneId { get; set; }

        [Required]
        public IEnumerable<ParkingSlot> ParkingSlots { get; set; }

        public SelectList ListOfZones { get; set; }

        public FreeSlotsVM() { }

        public FreeSlotsVM(IEnumerable<ParkingZone> parkingZones)
        {
            ListOfZones = new SelectList(parkingZones, "Id", "Name");
        }
    }
}
