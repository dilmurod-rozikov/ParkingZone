using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingZoneApp.Models.Entities
{
    [Table("Reservation")]
    [ComplexType]
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
        [MaxLength(20)]
        public string VehicleNumber { get; set; }

        [Required]
        public virtual ParkingSlot ParkingSlot { get; set; }

        [Required]
        public bool IsPaid { get; set; }

        [Required]
        [ForeignKey(nameof(ParkingZone))]
        public Guid ParkingZoneId { get; set; }

        [Required]
        [ForeignKey(nameof(ParkingSlot))]
        public Guid ParkingSlotId { get; set; }

        [Required]
        [MaxLength(100)]
        public virtual string UserId { get; set; }

        [NotMapped]
        public bool IsActive
        {
            get => StartingTime <= DateTime.Now && StartingTime.AddHours(Duration) > DateTime.Now;
        }
    }
}
