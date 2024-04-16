using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingZoneApp.Models
{
    [Table("ParkingZone")]
    public class ParkingZone
    {
        [Key]
        [Required]
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateOnly? CreatedDate { get; init; } = new DateOnly();
    }
}