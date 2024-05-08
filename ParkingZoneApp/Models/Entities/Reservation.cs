using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingZoneApp.Models.Entities
{
    [Table("Reservation")]
    public class Reservation
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public DateTime StartingTime { get; set; }

        [Required]
        [ForeignKey(nameof(ParkingZone))]
        public Guid ZoneId { get; set; }

        [Required]
        [ForeignKey(nameof(ParkingSlot))]
        public Guid SlotId { get; set; }

        [Required]
        public virtual ParkingSlot ParkingSlot { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}
