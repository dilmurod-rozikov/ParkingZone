using ParkingZoneApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingZoneApp.Models.Entities
{
    [Table("ParkingSlot")]
    [ComplexType]
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
        [ForeignKey(nameof(ParkingZone))]
        public Guid ParkingZoneId { get; set; }

        [Required]
        public virtual ParkingZone ParkingZone { get; set; }

        [Required]
        public virtual ICollection<Reservation> Reservations { get; set; }

        [NotMapped]
        public bool IsInUse
        {
            get => Reservations.Any(x => x.StartingTime.AddHours(x.Duration) > DateTime.Now);
        }
    }
}
