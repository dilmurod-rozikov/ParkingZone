using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ReservationVMs
{
    public class FreeSlotsVMs
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be higher than 0")]
        public int Duration { get; set; }

        [Required]
        public DateTime StartingTime { get; set; } = DateTime.Now;

        [Required]
        public Guid SelectedZoneId { get; set; }

        public IEnumerable<ParkingSlot> ParkingSlots { get; set; }

        public SelectList ListOfZones { get; set; }

        public FreeSlotsVMs()
        {
        }

        public FreeSlotsVMs(IEnumerable<ParkingZone> zones)
        {
            ListOfZones = new SelectList(zones, "Id", "Name");
        }
    }
}
