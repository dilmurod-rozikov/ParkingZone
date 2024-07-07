using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidation.Reservation
{
    public class ReserveVMTests
    {
        private static readonly ParkingSlot parkingSlot = new() { Reservations = [new() { Duration = 5 }] };
        public static IEnumerable<object[]> Data =>
            [
                [Guid.NewGuid(), 2, DateTime.Now.AddHours(10), "ZZ777Z", Guid.NewGuid(), Guid.NewGuid(), parkingSlot, true],
                [Guid.NewGuid(), 2, DateTime.Now.AddHours(10), null, Guid.NewGuid(), Guid.NewGuid(), parkingSlot, false],
            ];

        [Theory]
        [MemberData(nameof(Data))]
        public void GivenItemToBeValidated_WhenCreatingReserveVM_ThenValidationIsPerformed
            (Guid id, uint duration, DateTime startTime, string vehicleNumber, Guid slotId, Guid zoneId, ParkingSlot slot, bool expectedValidation)
        {
            //Arrange
            ReserveVM reserveVM = new()
            {
                Id = id,
                Duration = duration,
                StartingTime = startTime,
                VehicleNumber = vehicleNumber,
                SlotId = slotId,
                ZoneId = zoneId,
                ParkingSlot = slot,
            };

            var validationContext = new ValidationContext(reserveVM);
            var validationResult = new List<ValidationResult>();

            //Act
            bool result = Validator.TryValidateObject(reserveVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
    }
}
