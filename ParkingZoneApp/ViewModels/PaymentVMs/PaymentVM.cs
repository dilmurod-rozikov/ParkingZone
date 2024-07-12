using ParkingZoneApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.PaymentVMs
{
    public class PaymentVM
    {
        [Required]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be 16 digits.")]
        [MaxLength(16)]
        [MinLength(16)]
        public string CardNumber { get; set; }

        [Required]
        public DateOnly ExpirationDate { get; set; } = new(DateTime.Now.Year - 5, 1, 1);

        [Required]
        [MaxLength(4)]
        [MinLength(3)]
        public string CVV { get; set; }

        [Required]
        [MaxLength(100)]
        public string CardName { get; set; }

        [Required]
        public ParkingSlot ParkingSlot { get; set; }

        public Payment MapToModel()
        {
            return new()
            {
                CardName = CardName,
                CardNumber = CardNumber,
                CVV = CVV,
                ExpirationDate = ExpirationDate,
                ParkingSlot = ParkingSlot,
            };
        }
    }
}
