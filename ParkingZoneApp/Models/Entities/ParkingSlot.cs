using ParkingZoneApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingZoneApp.Models.Entities
{
    [Table("ParkingSlot")]
    public class ParkingSlot
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public SlotCategory Category { get; set; } 

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public Guid ParkingZoneId { get; set; }

        [Required]
        public ParkingZone ParkingZone { get; set; }
    }
}
