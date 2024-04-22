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
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required]
        public ICollection<ParkingSlot> ParkingSlots { get; set; }
    }
}