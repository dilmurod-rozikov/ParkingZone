using ParkingZoneApp.ViewModels.ParkingZones;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidationTests
{
    public class CreateVMTests
    {
        public static IEnumerable<object[]> testCreateVMData =>
            new List<object[]>
            {
                new object[] { Guid.NewGuid(), null, "Chilonzor", new DateOnly(2024, 4, 12) },
                new object[] { null, "7Parking", "Qoraqamish", new DateOnly(2024, 4, 12) },
                new object[] { Guid.NewGuid(), "Sharafshon", null, new DateOnly(2024, 4, 12) },
                new object[] { Guid.NewGuid(), "Sharafshon", "Andijon", null }
            };

        [Theory]
        [MemberData(nameof(testCreateVMData))]
        public void CreateVM_WithValidData_ShouldPassValidation(Guid? id, string name, string address, DateOnly? createdDate)
        {
            //Arrange
            CreateVM createVM = new CreateVM()
            {
                Id = id, 
                Name = name, 
                Address = address, 
                CreatedDate = createdDate
            };

            var validationContext = new ValidationContext(createVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var isValidResult = Validator.TryValidateObject(createVM, validationContext, validationResult);

            //Assert
            Assert.NotEmpty(validationResult);
            Assert.False(isValidResult);
        }
    }
}
