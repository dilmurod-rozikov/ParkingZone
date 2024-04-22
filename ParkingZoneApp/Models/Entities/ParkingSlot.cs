using ParkingZoneApp.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingZoneApp.Models.Entities
{
    [Table("ParkingSlot")]
    public class ParkingSlot
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public Category CategoryType { get; set; } = Category.Standard;

        [Required]
        public bool IsAvailable { get; set; } = true;

        [Required]
        public Guid ParkingZoneId { get; set; }
    }
}
