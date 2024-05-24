using ParkingZoneApp.Enums;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidation.ParkingSlots
{
    public class FilterSlotVMTests
    {
        public static IEnumerable<object[]> TestData =>
           [
                [Guid.NewGuid(), SlotCategory.Standard, true, true],
           ];

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingGetCurrentCarsVM_ThenValidationIsPerformed
            (Guid parkingZoneId, SlotCategory category, bool isSlotFree, bool expectedValidation)
        {
            //Arrange
            FilterSlotVM vm = new()
            {
                ParkingZoneId = parkingZoneId,
                Category = category,
                IsSlotFree = isSlotFree,
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
