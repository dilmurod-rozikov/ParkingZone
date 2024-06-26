using ParkingZoneApp.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingZoneApp.Models
{
    [Table("ParkingZone")]
    public class ParkingZone
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required]
        public virtual ICollection<ParkingSlot> ParkingSlots { get; set; }
    }
}