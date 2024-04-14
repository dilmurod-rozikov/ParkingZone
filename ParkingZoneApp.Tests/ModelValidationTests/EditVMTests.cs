using ParkingZoneApp.Models;
using ParkingZoneApp.ViewModels.ParkingZones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingZoneApp.Tests.ModelValidationTests
{
    public class EditVMTests
    {
        public static IEnumerable<object[]> testEditVMData =>
           new List<object[]>
           {
                new object[] { Guid.NewGuid(), null, "Chilonzor", new DateOnly(2024, 4, 12) },
                new object[] { null, "7Parking", "Qoraqamish", new DateOnly(2024, 4, 12) },
                new object[] { Guid.NewGuid(), "Sharafshon", null, new DateOnly(2024, 4, 12) },
                new object[] { Guid.NewGuid(), "Sharafshon", "Andijon", null }
           };

        [Theory]
        [MemberData(nameof(testEditVMData))]
        public void CreateVM_WithValidData_ShouldPassValidation(Guid? id, string name, string address, DateOnly? createdDate)
        {
            //Arrange
            ParkingZone parkingZone = new ParkingZone()
            {
                Id = id,
                Name = name,
                Address = address,
                CreatedDate = createdDate
            };

            EditVM editVM = new EditVM(parkingZone);
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
