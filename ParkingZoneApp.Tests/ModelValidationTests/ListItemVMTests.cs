using ParkingZoneApp.ViewModels.ParkingZones;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidationTests
{
    public class ListItemVMTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] { Guid.NewGuid(), null, "Test1", new DateOnly(2024, 4, 12), false },
                new object[] { Guid.NewGuid(), "Test3", null, new DateOnly(2024, 4, 12), false },
                new object[] { Guid.NewGuid(), "Test5", "Test5", new DateOnly(2024, 4, 12), true }
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingListItemVM_ThenValidationIsPerformed
            (Guid id, string name, string address, DateOnly createdDate, bool expectedValidation)
        {   
            //Arrange
            var listItemVM = new ListItemVM()
            {
                Id = id,
                Name = name,
                Address = address,
                CreatedDate = createdDate
            };

            var validationContext = new ValidationContext(listItemVM, null, null);
            var validationResults = new List<ValidationResult>();

            //Act
            bool result = Validator.TryValidateObject(listItemVM, validationContext, validationResults);

            //Assert
            Assert.Equal(expectedValidation, result);
        }

    }
}
