using ParkingZoneApp.ViewModels.ReservationVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidation.Reservation
{
    public class ProlongVMTests
    {
        public static IEnumerable<object[]> Data =>
        [
            [Guid.NewGuid(), 1, DateTime.Now, DateTime.Now.AddHours(2), true],
        ];

        [Theory]
        [MemberData(nameof(Data))]
        public void GivenItemToBeValidated_WhenCreatingReserveVM_ThenValidationIsPerformed
            (Guid id, uint duration, DateTime startTime, DateTime finishTime, bool expectedValidation)
        {
            //Arrange
            ProlongVM prolongVM = new()
            {
                Id = id,
                ProlongDuration = duration,
                StartTime = startTime,
                FinishTime = finishTime,
            };

            var validationContext = new ValidationContext(prolongVM);
            var validationResult = new List<ValidationResult>();

            //Act
            bool result = Validator.TryValidateObject(prolongVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
    }
}
