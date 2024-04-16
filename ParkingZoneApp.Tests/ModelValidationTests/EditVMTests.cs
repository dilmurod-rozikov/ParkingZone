using ParkingZoneApp.Models;
using ParkingZoneApp.ViewModels.ParkingZones;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidationTests
{
    public class EditVMTests
    {
        public static IEnumerable<object[]> TestEditVMData =>
           new List<object[]>
           {
                new object[] { Guid.NewGuid(), null, "Chilonzor", new DateOnly(2024, 4, 12) },
                new object[] { null, "7Parking", "Qoraqamish", new DateOnly(2024, 4, 12) },
                new object[] { Guid.NewGuid(), "Sharafshon", null, new DateOnly(2024, 4, 12) },
                new object[] { Guid.NewGuid(), "Sharafshon", "Andijon", null }
           };

        [Theory]
        [MemberData(nameof(TestEditVMData))]
        public void GivenValidData_WhenCreatingEditVM_ThenValidationShouldFail(Guid? id, string name, string address, DateOnly? createdDate)
        {
            //Arrange
            EditVM editVM = new EditVM()
            {
                Id = id,
                Name = name,
                Address = address,
                CreatedDate = createdDate
            };

            var validationContext = new ValidationContext(editVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var isValidResult = Validator.TryValidateObject(editVM, validationContext, validationResult);

            //Assert
            Assert.NotEmpty(validationResult);
            Assert.False(isValidResult);
        }
    }
}
