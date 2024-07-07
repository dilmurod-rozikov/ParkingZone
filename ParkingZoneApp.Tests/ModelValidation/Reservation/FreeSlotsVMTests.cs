using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidation.Reservation
{
    public class FreeSlotsVMTests
    {
        public static IEnumerable<object[]> Data =>
            [
                [Guid.NewGuid(), 2u, DateTime.Now, Guid.NewGuid(), new List<ParkingSlot>(), true],
            ];

        [Theory]
        [MemberData(nameof(Data))]
        public void GivenItemToBeValidated_WhenCreatingFreeSlotsVM_ThenValidationIsPerformed
            (Guid id, uint duration, DateTime startTime, Guid selectedZoneId, IEnumerable<ParkingSlot> slots, bool expectedValidation)
        {
            //Arrange
            FreeSlotsVM freeSlotsVMs = new()
            {
                Id = id,
                Duration = duration,
                StartingTime = startTime,
                SelectedZoneId = selectedZoneId,
                ParkingSlots = slots,
            };

            var validationContext = new ValidationContext(freeSlotsVMs);
            var validationResult = new List<ValidationResult>();

            //Act
            bool result = Validator.TryValidateObject(freeSlotsVMs, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
    }
}
