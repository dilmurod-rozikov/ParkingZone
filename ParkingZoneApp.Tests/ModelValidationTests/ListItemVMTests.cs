using ParkingZoneApp.ViewModels.ParkingZones;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidationTests
{
    public class ListItemVMTests
    {
        public static IEnumerable<object[]> TestListItemVMData =>
            new List<object[]>
            {
                new object[] { Guid.NewGuid(), null, "Chilonzor", new DateOnly(2024, 4, 12) },
                new object[] { null, "7Parking", "Qoraqamish", new DateOnly(2024, 4, 12) },
                new object[] { Guid.NewGuid(), "Sharafshon", null, new DateOnly(2024, 4, 12) },
                new object[] { Guid.NewGuid(), "Sharafshon", "Andijon", null }
            };

        [Theory]
        [MemberData(nameof(TestListItemVMData))]
        public void GivenValidData_WhenCreatingListItemVM_ThenShouldPassValidation(Guid? id, string name, string address, DateOnly? createdDate)
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
            bool isValidResult = Validator.TryValidateObject(listItemVM, validationContext, validationResults, true);

            //Assert
            Assert.False(isValidResult);
            Assert.NotEmpty(validationResults);
        }

    }
}
