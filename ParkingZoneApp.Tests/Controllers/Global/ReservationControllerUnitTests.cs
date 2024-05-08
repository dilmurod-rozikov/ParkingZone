using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingZoneApp.Controllers;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.Text.Json;

namespace ParkingZoneApp.Tests.Controllers.Global
{
    public class ReservationControllerUnitTests
    {
        private readonly Mock<IReservationService> _reservationServiceMock;
        private readonly Mock<IParkingSlotService> _slotServiceMock;
        private readonly Mock<IParkingZoneService> _zoneServiceMock;
        private readonly ReservationController _controller;
        private static readonly ParkingZone parkingZone = new()
        {
            Id = Guid.NewGuid(),
            Name = "name",
            Address = "address",
            CreatedDate = new DateOnly(),
            ParkingSlots = [new()]
        };

        private static readonly ParkingSlot parkingSlot = new()
        {
            Id = Guid.NewGuid(),
            Number = 1,
            Category = SlotCategory.Standard,
            IsAvailable = true,
            ParkingZoneId = parkingZone.Id,
            ParkingZone = parkingZone
        };
        public static readonly IEnumerable<ParkingZone> parkingZones = [ parkingZone ];
        public static readonly IEnumerable<ParkingSlot> parkingSlots = [ parkingSlot ];

        public ReservationControllerUnitTests()
        {
            _reservationServiceMock = new Mock<IReservationService>();
            _slotServiceMock = new Mock<IParkingSlotService>();
            _zoneServiceMock = new Mock<IParkingZoneService>();
            _controller = new ReservationController(_reservationServiceMock.Object, _zoneServiceMock.Object, _slotServiceMock.Object);
        }

        #region FreeSlots
        [Fact]
        public void GivenNothing_WhenGetFreeSlotsIsCalled_ThenReturnViewResult()
        {
            //Arrange
            var expected = new List<FreeSlotsVMs>()
            {
                new(parkingZones)
            };
            _zoneServiceMock.Setup(x => x.GetAll()).Returns(parkingZones);
            //Act
            var result = _controller.FreeSlots();

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(model), JsonSerializer.Serialize(expected[0]));
            _zoneServiceMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public void GivenFreeSlotsVM_WhenPostFreeSlotsIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            FreeSlotsVMs freeSlotsVMs = new()
            {
                Id = Guid.NewGuid(),
                Duration = 1,
                StartingTime = DateTime.Now,
                SelectedZoneId = parkingZone.Id
            };

            _zoneServiceMock.Setup(x => x.GetAll()).Returns(parkingZones);
            _slotServiceMock.Setup(x => x
                .GetAllFreeSlots(freeSlotsVMs.SelectedZoneId, freeSlotsVMs.StartingTime, freeSlotsVMs.Duration))
                .Returns(parkingSlots);
            //Act
            var result = _controller.FreeSlots(freeSlotsVMs);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(model), JsonSerializer.Serialize(freeSlotsVMs));
            _zoneServiceMock.Verify(x => x.GetAll(), Times.Once);
            _slotServiceMock.Verify(x => x.GetAllFreeSlots
                (freeSlotsVMs.SelectedZoneId, freeSlotsVMs.StartingTime, freeSlotsVMs.Duration), Times.Once);
        }
        #endregion
    }
}
