using ParkingZoneApp.ViewModels.ParkingZones;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidationTests.ParkingZones
{
    public class DetailVMTests
    {
        public static IEnumerable<object[]> TestData =>
           new List<object[]>
           {
                new object[] { Guid.NewGuid(), null, "Test1", new DateOnly(2024, 4, 12), false },
                new object[] { null, "Test2", "Test2", new DateOnly(2024, 4, 12), false },
                new object[] { Guid.NewGuid(), "Test3", null, new DateOnly(2024, 4, 12), false },
                new object[] { Guid.NewGuid(), "Test4", "Test4", null, false },
                new object[] { Guid.NewGuid(), "Test5", "Test5", new DateOnly(2024, 4, 12), true }
           };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingDetailsVM_ThenValidationIsPerformed
            (Guid? id, string name, string address, DateOnly? createdDate, bool expectedValidation)
        {
            //Arrange
            DetailsVM detailsVM = new DetailsVM()
            {
                Id = id,
                Name = name,
                Address = address,
                CreatedDate = createdDate
            };

            var validationContext = new ValidationContext(detailsVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var isValidResult = Validator.TryValidateObject(detailsVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, isValidResult);
        }
    }
}
