using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidation.Reservation
{
    public class ReserveVMTests
    {
        public static IEnumerable<object[]> Data =>
            [
                [Guid.NewGuid(), 1, DateTime.Now, "ZZ777Z", Guid.NewGuid(), Guid.NewGuid(), new ParkingSlot(), true],
                [Guid.NewGuid(), 1, DateTime.Now, null, Guid.NewGuid(), Guid.NewGuid(), new ParkingSlot(), false],
                [Guid.NewGuid(), 1, DateTime.Now, "null", Guid.NewGuid(), Guid.NewGuid(), null, false],
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
