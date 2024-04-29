using ParkingZoneApp.Enums;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Tests.ModelValidation.ParkingSlots
{
    public class CreateVMTests
    {
        public static IEnumerable<object[]> TestData =>
             new List<object[]>
             {
                 new object[] { 20, SlotCategory.VIP, true, Guid.NewGuid(), true },
             };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingCreateVM_ThenValidationIsPerformed
            (int number, SlotCategory category, bool isAvailable, Guid id, bool expectedValidation)
        {
            //Arrange
            CreateVM createVM = new CreateVM()
            {
                Number = number,
                Category = category,
                IsAvailable = isAvailable,
                ParkingZoneId = id,
            };

            var validationContext = new ValidationContext(createVM);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(createVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
    }
}
