using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingZoneApp.Areas.Admin.Models
{
    [Table("ParkingZone")]
    public class ParkingZone
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateOnly CreatedDate { get; init; } = new DateOnly();
    }
}
