using ParkingZoneApp.ViewModels.ReservationVMs;
using System.ComponentModel.DataAnnotations;
namespace ParkingZoneApp.Tests.ModelValidation.Reservation
{
    public class IndexVMTests
    {
        public static IEnumerable<object[]> Data =>
            [
               [new DateTime(), 1, 12, "ZoneAddress", "ZoneName", "VehicleNumber", true],
               [new DateTime(), 1, 12, null, "ZoneName", "VehicleNumber", false],
               [new DateTime(), 1, 12, "ZoneAddress", null, "VehicleNumber", false],
               [new DateTime(), 1, 12, "ZoneAddress", "ZoneName", null, false],
            ];

        [Theory]
        [MemberData(nameof(Data))]
        public void GivenItemToBeValidated_WhenCreatingReserveVM_ThenValidationIsPerformed
            (DateTime startTime, uint duration, int slotnumber, string address, string name, string number, bool expectedValidation)
        {
            //Arrage
            IndexVM indexVM = new()
            {
                StartDate = startTime,
                Duration = duration,
                SlotNumber = slotnumber,
                ZoneAddress = address,
                ZoneName = name,
                VehicleNumber = number,
            };

        var validationContext = new ValidationContext(indexVM);
        var validationResult = new List<ValidationResult>();

        //Act
        bool result = Validator.TryValidateObject(indexVM, validationContext, validationResult);

        //Assert
        Assert.Equal(expectedValidation, result);
        }
    }
}
