﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using ParkingZoneApp.Controllers;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.Security.Claims;
using System.Text.Json;

namespace ParkingZoneApp.Tests.Controllers.Global
{
    public class ReservationControllerUnitTests
    {
        private readonly Mock<IReservationService> _reservationServiceMock = new();
        private readonly Mock<IParkingSlotService> _slotServiceMock;
        private readonly Mock<IParkingZoneService> _zoneServiceMock;
        private readonly ReservationController _controller;
        private static readonly Guid parkingSlotId = Guid.NewGuid();
        private static readonly Guid parkingZoneId = Guid.NewGuid();

        private static readonly ParkingSlot parkingSlot = new()
        {
            Id = parkingSlotId,
            Number = 1,
            Category = SlotCategory.Standard,
            IsAvailable = true,
            ParkingZoneId = parkingZoneId,
            ParkingZone = parkingZone,
            Reservations =
            [
                new()
                {
                    Id = Guid.NewGuid(),
                    StartingTime = DateTime.Now,
                    Duration = 2,
                    VehicleNumber = "",
                    ParkingSlotId = parkingSlotId,
                    ParkingZoneId = parkingZoneId
                }
            ]
        };
        public static readonly IEnumerable<ParkingSlot> parkingSlots = [parkingSlot];
        private static readonly ParkingZone parkingZone = new()
        {
            Id = parkingZoneId,
            Name = "name",
            Address = "address",
            CreatedDate = new DateOnly(),
            ParkingSlots = parkingSlots.ToList(),
        };

        public static readonly IEnumerable<ParkingZone> parkingZones = [parkingZone];
        public ReservationControllerUnitTests()
        {
            _slotServiceMock = new Mock<IParkingSlotService>();
            _zoneServiceMock = new Mock<IParkingZoneService>();
            _controller = new ReservationController(_zoneServiceMock.Object, _slotServiceMock.Object);
        }

        #region FreeSlots
        [Fact]
        public async Task GivenNothing_WhenGetFreeSlotsIsCalled_ThenReturnViewResult()
        {
            //Arrange
            var expected = new List<FreeSlotsVM>()
            {
                new(parkingZones)
            };
            _zoneServiceMock.Setup(x => x.GetAll()).ReturnsAsync(parkingZones);
            //Act
            var result = await _controller.FreeSlots();

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(model), JsonSerializer.Serialize(expected[0]));
            _zoneServiceMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GivenFreeSlotsVM_WhenPostFreeSlotsIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            FreeSlotsVM freeSlotsVMs = new()
            {
                Id = Guid.NewGuid(),
                Duration = 1,
                StartingTime = DateTime.Now,
                SelectedZoneId = parkingZone.Id
            };

            _zoneServiceMock.Setup(x => x.GetAll()).ReturnsAsync(parkingZones);
            _slotServiceMock.Setup(x => x
                .GetAllFreeSlotsAsync(freeSlotsVMs.SelectedZoneId, freeSlotsVMs.StartingTime, freeSlotsVMs.Duration))
                .ReturnsAsync(parkingSlots);
            //Act
            var result = await _controller.FreeSlots(freeSlotsVMs);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(model), JsonSerializer.Serialize(freeSlotsVMs));
            _zoneServiceMock.Verify(x => x.GetAll(), Times.Once);
            _slotServiceMock.Verify(x => x.GetAllFreeSlotsAsync
                (freeSlotsVMs.SelectedZoneId, freeSlotsVMs.StartingTime, freeSlotsVMs.Duration), Times.Once);
        }
        #endregion

        #region Reserve
        [Fact]
        public async Task GivenSlotIdAndStartTimeAndDuration_WhenGetReserveIsCalled_ThenReturnsNotFoundResult()
        {
            //Arrage
            _slotServiceMock.Setup(x => x.GetById(parkingSlot.Id));

            //Act
            var result = await _controller.Reserve(parkingSlot.Id, DateTime.Now, 5);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            _slotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
        }

        [Fact]
        public async Task GivenSlotIdAndStartTimeAndDuration_WhenGetReserveIsCalled_ThenReturnsViewResult()
        {
            //Arrage
            ReserveVM reserveVM = new(5, new DateTime(2024, 5, 9, 11, 11, 11, 0, 0), parkingSlot.Id,
                    parkingZone.Id, parkingZone.Name, parkingZone.Address, parkingSlot.Number);

            _slotServiceMock.Setup(x => x.GetById(parkingSlot.Id)).ReturnsAsync(parkingSlot);
            _zoneServiceMock.Setup(x => x.GetById(parkingZone.Id)).ReturnsAsync(parkingZone);

            //Act
            var result = await _controller.Reserve(parkingSlot.Id, new DateTime(2024, 5, 9, 11, 11, 11, 0, 0), 5);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(JsonSerializer.Serialize(reserveVM), JsonSerializer.Serialize(((ViewResult)result).Model));
            _zoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
            _slotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
        }

        [Fact]
        public async Task GivenReserveVM_WhenPostReserveIsCalled_ThenReturnsModelErrorForOccupiedReservation()
        {
            //Arrage
            ReserveVM reserveVM = new()
            {
                Id = Guid.NewGuid(),
                SlotId = parkingSlot.Id,
                StartingTime = new DateTime(3003, 5, 9, 11, 11, 0, 0, 0),
                Duration = 5,
                VehicleNumber = "DDD000",
                ZoneId = parkingZone.Id,
            };
            _slotServiceMock.Setup(x => x.GetById(parkingSlotId)).ReturnsAsync(parkingSlot);
            _zoneServiceMock.Setup(x => x.GetById(parkingZoneId)).ReturnsAsync(parkingZone);
            _slotServiceMock
                .Setup(x => x.IsSlotFreeForReservation(parkingSlot, reserveVM.StartingTime, reserveVM.Duration))
                .Returns(false);

            _controller.ModelState.AddModelError("StartingTime", "Slot is not free for selected period");

            //Act
            var result = await _controller.Reserve(reserveVM, _reservationServiceMock.Object);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.Equal(JsonSerializer.Serialize(model), JsonSerializer.Serialize(reserveVM));
            Assert.False(_controller.ModelState.IsValid);
            _slotServiceMock.Verify(x => x.GetById(parkingSlotId), Times.Once());
            _zoneServiceMock.Verify(x => x.GetById(parkingZoneId), Times.Once());
            _slotServiceMock.Verify(x => x.IsSlotFreeForReservation
                        (parkingSlot, reserveVM.StartingTime, reserveVM.Duration), Times.Once);
        }

        [Fact]
        public async Task GivenReserveVM_WhenPostReserveIsCalled_ThenReturnsModelErrorForVehicleNumber()
        {
            //Arrage
            ReserveVM reserveVM = new()
            {
                SlotId = parkingSlot.Id,
                StartingTime = new DateTime(2024, 5, 9, 11, 11, 0, 0, 0),
                Duration = 5,
                ZoneId = parkingZone.Id,
            };
            _slotServiceMock.Setup(x => x.GetById(parkingSlotId)).ReturnsAsync(parkingSlot);
            _zoneServiceMock.Setup(x => x.GetById(parkingZoneId)).ReturnsAsync(parkingZone);
            _slotServiceMock
                .Setup(x => x.IsSlotFreeForReservation(parkingSlot, reserveVM.StartingTime, reserveVM.Duration))
                .Returns(true);

            _controller.ModelState.AddModelError("VehicleNumber", "Vehicle Number is required.");

            //Act
            var result = await _controller.Reserve(reserveVM, _reservationServiceMock.Object);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.Equal(JsonSerializer.Serialize(model), JsonSerializer.Serialize(reserveVM));
            Assert.False(_controller.ModelState.IsValid);
            _slotServiceMock.Verify(x => x.GetById(parkingSlotId), Times.Once());
            _zoneServiceMock.Verify(x => x.GetById(parkingZoneId), Times.Once());
            _slotServiceMock.Verify(x => x.IsSlotFreeForReservation
                        (parkingSlot, reserveVM.StartingTime, reserveVM.Duration), Times.Once);
        }

        [Fact]
        public async Task GivenReserveVM_WhenPostReserveIsCalled_ThenReturnsViewResult()
        {
            //Arrage
            ReserveVM reserveVM = new()
            {
                SlotId = parkingSlot.Id,
                StartingTime = new DateTime(2024, 5, 9, 11, 11, 0, 0, 0),
                Duration = 5,
                VehicleNumber = "DDD000",
                ZoneId = parkingZone.Id,
            };
            var mockClaimsPrincipal = CreateMockClaimsPrincipal();

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockClaimsPrincipal }
            };
            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            _slotServiceMock.Setup(x => x.GetById(parkingSlotId)).ReturnsAsync(parkingSlot);
            _zoneServiceMock.Setup(x => x.GetById(parkingZoneId)).ReturnsAsync(parkingZone);
            _reservationServiceMock.Setup(x => x.Insert(It.IsAny<Reservation>()));
            _slotServiceMock
                .Setup(x => x.IsSlotFreeForReservation(parkingSlot, reserveVM.StartingTime, reserveVM.Duration))
                .Returns(true);

            //Act
            var result = await _controller.Reserve(reserveVM, _reservationServiceMock.Object);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.Equal("Parking slot reserved successfully.", _controller.TempData["ReservationSuccess"]);
            _slotServiceMock.Verify(x => x.GetById(parkingSlotId), Times.Once());
            _zoneServiceMock.Verify(x => x.GetById(parkingZoneId), Times.Once());
            _reservationServiceMock.Verify(x => x.Insert(It.IsAny<Reservation>()), Times.Once());
            _slotServiceMock.Verify(x => x.IsSlotFreeForReservation
                        (parkingSlot, reserveVM.StartingTime, reserveVM.Duration), Times.Once);
        }

        private static ClaimsPrincipal CreateMockClaimsPrincipal()
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, "UserId")
            };
            var identity = new ClaimsIdentity(claims);
            return new ClaimsPrincipal(identity);
        }
        #endregion
    }
}
