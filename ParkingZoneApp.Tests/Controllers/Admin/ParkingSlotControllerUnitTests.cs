using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingZoneApp.Areas.Admin.Controllers;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ParkingSlots;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
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
            Category = SlotCategory.Standard,
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

        #region
        [Fact]
        public void GivenParkingZoneId_WhenCreateGetIsCalled_ThenReturnViewResult()
        {
            //Arrange
            CreateVM createVM = new CreateVM()
            {
                ParkingZoneId = parkingZone.Id,
            };

            //Act
            var result = _controller.Create(createVM);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(createVM.ParkingZoneId, parkingZone.Id);
        }

        [Fact]
        public void GivenParkingSlotId_WhenCreatePostIsCalled_ThenReturnModelErrorIfNumberIsNotUnique()
        {
            //Arrange
            CreateVM createVM = new()
            {
                Number = parkingSlot.Number,
                Category = parkingSlot.Category,
                ParkingZoneId = parkingSlot.ParkingZoneId,
                IsAvailable = parkingSlot.IsAvailable
            };
            _parkingSlotServiceMock.Setup(x => x.IsUniqueNumber(createVM.ParkingZoneId, createVM.Number)).Returns(false);

            //Act
            var result = _controller.Create(createVM);

            //Assert
            Assert.NotNull(result);
            _parkingSlotServiceMock.Verify(x => x.IsUniqueNumber(parkingZone.Id, parkingSlot.Number), Times.Once);
        }

        [Fact]
        public void GivenCreateVM_WhenCreatePostIsCalled_ThenThenModelStateIsFalseAndReturnViewResult()
        {
            //Arrange
            CreateVM createVM = new();
            _controller.ModelState.AddModelError("Number", "Number is required");

            //Act
            var result = _controller.Create(createVM);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsAssignableFrom<CreateVM>(model);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Equal(JsonSerializer.Serialize(createVM), JsonSerializer.Serialize(model));
        }

        [Fact]
        public void GivenCreateVM_WhenCreatePostIsCalled_ThenThenModelStateIsTrueAndReturnRedirectToIndex()
        {
            //Arrange
            var createVM = new CreateVM()
            {
                Number = 1,
                IsAvailable = true,
                Category = SlotCategory.VIP,
                ParkingZoneId = parkingZone.Id,
            };

            _parkingSlotServiceMock
                    .Setup(x => x.Insert(parkingSlot));

            //Act
            var result = _controller.Create(createVM);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
            Assert.True(_controller.ModelState.IsValid);
            _parkingSlotServiceMock.Verify(service => service.Insert(It.IsAny<ParkingSlot>()), Times.Once);
        }
        #endregion
    }
}
