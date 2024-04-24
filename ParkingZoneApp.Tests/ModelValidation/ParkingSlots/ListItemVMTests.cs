using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Models.Enums;
using ParkingZoneApp.ViewModels.ParkingSlots;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidationTests.ParkingSlots
{
    public class ListItemVMTests
    {
        public static IEnumerable<object[]> TestData =>
           new List<object[]>
           {
                new object[] { Guid.NewGuid(), 3, new SlotCategory(), false, null, false },
                new object[] { Guid.NewGuid(), 4, new SlotCategory(), false, new ParkingZone(), true },
           };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingListItemVM_ThenValidationIsPerformed
            (Guid id, int number, SlotCategory category, bool isAvailable, ParkingZone parkingZone, bool expectedValidation)
        {
            //Arrange
            ListItemVM listItemVM = new ListItemVM()
            {
                Id = id,
                Number = number,
                Category = category,
                IsAvailable = isAvailable,
                ParkingZone = parkingZone,
            };

            var validationContext = new ValidationContext(listItemVM);
            var validationResult = new List<ValidationResult>();

            //Act
            bool result = Validator.TryValidateObject(listItemVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
       
    }
}
