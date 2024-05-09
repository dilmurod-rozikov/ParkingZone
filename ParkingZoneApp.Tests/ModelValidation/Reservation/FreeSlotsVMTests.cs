using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidation.Reservation
{
    public class FreeSlotsVMTests
    {
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] {Guid.NewGuid(), 2, DateTime.Now, Guid.NewGuid(), new List<ParkingSlot>(), true },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void GivenItemToBeValidated_WhenCreatingFreeSlotsVM_ThenValidationIsPerformed
            (Guid id, int duration, DateTime startTime, Guid selectedZoneId, IEnumerable<ParkingSlot> slots, bool expectedValidation)
        {
            //Arrange
            FreeSlotsVMs freeSlotsVMs = new()
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
