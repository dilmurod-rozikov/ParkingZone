using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingZoneApp.Areas.Admin.Controllers;
using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ParkingSlots;
using System.Text.Json;

namespace ParkingZoneApp.Tests.Controllers.Admin
{
    public class ParkingSlotControllerUnitTests
    {
        private readonly Mock<IParkingSlotService> _parkingSlotServiceMock;
        private readonly ParkingSlotController _controller;
        private static readonly ParkingZone parkingZone = new()
        {
            Id = Guid.NewGuid(),
            Name = "name",
            Address = "address",
            CreatedDate = new DateOnly(),
            ParkingSlots = new List<ParkingSlot>()
            {
                new()
            }
        };

        private readonly ParkingSlot parkingSlot = new()
        {
            Id = Guid.NewGuid(),
            Number = 1,
            Category = Models.Enums.SlotCategory.Standard,
            IsAvailable = true,
            ParkingZoneId = parkingZone.Id,
            ParkingZone = parkingZone
        };

        public ParkingSlotControllerUnitTests()
        {
            _parkingSlotServiceMock =new Mock<IParkingSlotService>();
            _controller = new ParkingSlotController(_parkingSlotServiceMock.Object);
        }

        #region Index
        [Fact]
        public void GivenParkingZoneId_WhenIndexIsCalled_ThenReturnViewResult()
        {
            //Arrange
            var testSlots = new List<ParkingSlot>()
            {
                parkingSlot,
            };

            var expectedListOfItems = new List<ListItemVM>()
            {
                new ListItemVM(parkingSlot) { }
            };

            _parkingSlotServiceMock
                    .Setup(x => x.GetSlotsByZoneId(parkingZone.Id))
                    .Returns(testSlots);
            //Act
            var result = _controller.Index(parkingZone.Id);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(model), JsonSerializer.Serialize(expectedListOfItems));
            _parkingSlotServiceMock.Verify(x => x.GetSlotsByZoneId(parkingZone.Id), Times.Once);
            _parkingSlotServiceMock.VerifyNoOtherCalls();
        }
        #endregion
    }
}
