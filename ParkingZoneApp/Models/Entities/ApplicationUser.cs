using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Name")]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
