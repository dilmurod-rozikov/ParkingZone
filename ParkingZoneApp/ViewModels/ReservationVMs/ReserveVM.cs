using ParkingZoneApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ReservationVMs
{
    public class ReserveVM
    {
        private static readonly DateTime now = DateTime.Now;

        [Required]
        public Guid Id { get; set; }

        [Required]
        public uint Duration { get; set; }

        [Required]
        public DateTime StartingTime { get; set; } = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

        [Required]
        public string VehicleNumber { get; set; }

        [Required]
        public Guid ZoneId { get; set; }

        [Required]
        public Guid SlotId { get; set; }

        [Required]
        public virtual ParkingSlot ParkingSlot { get; set; }

        public string ZoneName { get; set; }

        public string ZoneAddress {  get; set; }

        public int SlotNumber { get; set; }

        public ReserveVM() {  }

        public ReserveVM(uint duration, DateTime startTime, Guid slotId, Guid zoneId, string zoneName, string zoneAddress, int slotNumber)
        {
            Duration = duration;
            StartingTime = startTime;
            SlotId = slotId;
            ZoneId = zoneId;
            ZoneName = zoneName;
            ZoneAddress = zoneAddress;
            SlotNumber = slotNumber;
        }

        public Reservation MapToModel() =>
            new()
            {
                Id = Id,
                Duration = Duration,
                StartingTime = StartingTime,
                ParkingSlotId = SlotId,
                ParkingZoneId = ZoneId,
                VehicleNumber = VehicleNumber,
            };
    }
}
