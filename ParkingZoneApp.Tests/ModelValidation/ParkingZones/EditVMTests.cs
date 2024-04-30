using ParkingZoneApp.ViewModels.ParkingZoneVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidationTests.ParkingZones
{
    public class EditVMTests
    {
        public static IEnumerable<object[]> TestData =>
           new List<object[]>
           {
                new object[] { Guid.NewGuid(), null, "Test1", false },
                new object[] { Guid.NewGuid(), "Test3", null, false },
                new object[] { Guid.NewGuid(), "Test5", "Test5", true }
           };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingEditVM_ThenValidationIsPerformed
            (Guid id, string name, string address, bool expectedValidation)
        {
            //Arrange
            EditVM editVM = new()
            {
                Id = id,
                Name = name,
                Address = address,
            };

            var validationContext = new ValidationContext(editVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(editVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
    }
}
