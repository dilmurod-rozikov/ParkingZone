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
        public uint Duration { get; set; }

        [Required]
        public DateTime StartingTime { get; set; }

        [Required]
        public string VehicleNumber { get; set; }

        [Required]
        [ForeignKey(nameof(ParkingZone))]
        public Guid ParkingZoneId { get; set; }

        [Required]
        [ForeignKey(nameof(ParkingSlot))]
        public Guid ParkingSlotId { get; set; }

        [Required]
        public virtual string UserId { get; set; }
    }
}
