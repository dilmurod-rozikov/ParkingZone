using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingZoneApp.Models.Entities
{
    [Table("PaymentDetails")]
    public class Payment
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool PaymentStatus { get; set; }

        [Required]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be 16 digits.")]
        [MaxLength(16)]
        [MinLength(16)]
        public string CardNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string CardName { get; set; }

        [Required]
        [MaxLength(4)]
        [MinLength(3)]
        public string CVV { get; set; }

        [Required]
        public DateOnly ExpirationDate { get; set; } = new(DateTime.Now.Year - 5, 1, 1);

        [Required]
        public virtual ParkingSlot ParkingSlot { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
