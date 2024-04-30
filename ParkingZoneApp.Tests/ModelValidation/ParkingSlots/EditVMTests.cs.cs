using ParkingZoneApp.Enums;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
using System.ComponentModel.DataAnnotations;
namespace ParkingZoneApp.Tests.ModelValidation.ParkingSlots
{
    public class EditVMTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] { Guid.NewGuid(), 12, SlotCategory.VIP, true, Guid.NewGuid(), true},
                new object[] { null, 1, SlotCategory.VIP, true, Guid.NewGuid(), false },
                new object[] { Guid.NewGuid(), null, SlotCategory.VIP, true, Guid.NewGuid(), false },
                new object[] { Guid.NewGuid(), 1, null, true, Guid.NewGuid(), false },
                new object[] { Guid.NewGuid(), 1, SlotCategory.VIP, true, null, false },
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingEditVM_ThenValidationIsPerformed
            (Guid id, int number, SlotCategory category, bool isAvailable, Guid parkingZoneId, bool expectedValidation)
        {
            //Arrange
            EditVM editVM = new()
            {
                Id = id,
                Number = number,
                Category = category,
                IsAvailable = isAvailable,
                ParkingZoneId = parkingZoneId,
            };

            var validationContext = new ValidationContext(editVM, null, null);
            var validationResult = new List<ValidationResult>();
            //Act
            var result = Validator.TryValidateObject(editVM, validationContext, validationResult);

            //Assert
            Assert.Equal(result, expectedValidation);
        }
    }
}
