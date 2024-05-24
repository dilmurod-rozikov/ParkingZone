using ParkingZoneApp.ViewModels.ParkingZoneVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidation.ParkingZones
{
    public class GetCurrentCarsTests
    {
        public static IEnumerable<object[]> TestData =>
           new List<object[]>
           {
                new object[] { 1, "Test1", true },
           };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingGetCurrentCarsVM_ThenValidationIsPerformed
            (int number, string vehicleNumber, bool expectedValidation)
        {
            //Arrange
            GetCurrentCarsVM vm = new()
            {
                VehicleNumber = vehicleNumber,
                Number = number,
            };

            var validationContext = new ValidationContext(vm, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(vm, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
    }
}
