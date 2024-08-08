using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using ParkingZoneApp.Areas.User.Controllers;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.Security.Claims;
using System.Text.Json;

namespace ParkingZoneApp.Tests.Controllers.User
{
    public class ReservationControllerUnitTests
    {
        private readonly Mock<IReservationService> _reservationServiceMock;
        private readonly Mock<IParkingSlotService> _slotServiceMock;
        private readonly ReservationController _controller;
        private static readonly Guid parkingSlotId = Guid.NewGuid();
        private static readonly Guid parkingZoneId = Guid.NewGuid();
        private static readonly Reservation reservation = new()
        {
            Id = Guid.NewGuid(),
            Duration = 2,
            StartingTime = DateTime.Now,
            ParkingSlotId = parkingSlotId,
            ParkingZoneId = parkingZoneId,
            VehicleNumber = "DDD777",
            UserId = "UserId"
        };
        public static readonly IEnumerable<Reservation> reservations = [reservation];
        private static readonly ParkingSlot parkingSlot = new()
        {
            Id = parkingSlotId,
            Number = 1,
            Category = SlotCategory.Standard,
            IsAvailable = true,
            ParkingZoneId = parkingZoneId,
            ParkingZone = new(),
            Reservations = reservations.ToList(),
        };
        private readonly ParkingZone parkingZone = new()
        {
            Id = parkingZoneId,
            Name = "Test Name",
            Address = "Test Address",
            ParkingSlots =
            [
                new()
                {
                    Id = parkingSlotId,
                    ParkingZoneId = parkingZoneId,
                    Category = SlotCategory.Business,
                    Reservations = [reservation],
                    IsAvailable = false,
                    Number = 1,
                },
            ]
        };
        public ReservationControllerUnitTests()
        {
            _reservationServiceMock = new Mock<IReservationService>();
            _slotServiceMock = new Mock<IParkingSlotService>();
            _controller = new ReservationController(_reservationServiceMock.Object, _slotServiceMock.Object);
        }

        #region Index
        [Fact]
        public async Task GivenNothing_WhenGetIndexIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            Mock<IParkingZoneService> _zoneServiceMock = new();
            _slotServiceMock.Setup(x => x.GetAll()).ReturnsAsync([parkingSlot]);
            _zoneServiceMock.Setup(x => x.GetAll()).ReturnsAsync([parkingZone]);
            var mockClaimsPrincipal = CreateMockClaimsPrincipal();

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockClaimsPrincipal }
            };

            _reservationServiceMock.Setup(x => x.GetReservationsByUserId(It.IsAny<string>())).ReturnsAsync(reservations);
            //Act
            var result = await _controller.Index(_zoneServiceMock.Object);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            _reservationServiceMock.Verify((x => x.GetReservationsByUserId(It.IsAny<string>())), Times.Once);
            _zoneServiceMock.Verify(x => x.GetAll(), Times.Once);
            _slotServiceMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GivenNothing_WhenGetIndexIsCalled_ThenNotFoundResult()
        {
            //Arrange
            Mock<IParkingZoneService> _zoneServiceMock = new();
            reservation.UserId = null;
            _controller.ControllerContext = new();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal()
            };

            //Act
            var result = await _controller.Index(_zoneServiceMock.Object);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        private ClaimsPrincipal CreateMockClaimsPrincipal()
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, "UserId")
            };
            var identity = new ClaimsIdentity(claims);
            return new ClaimsPrincipal(identity);
        }
        #endregion

        #region Prolong
        [Fact]
        public async Task GivenReservationId_WhenGetProlongIsCalled_ThenReturnsNotFoundResult()
        {
            //Arrange
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id));

            //Act
            var result = await _controller.Prolong(reservation.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _reservationServiceMock.Verify(x => x.GetById(reservation.Id), Times.Once);
        }

        [Fact]
        public async Task GivenReservationId_WhenGetProlongIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            ProlongVM prolongVM = new()
            {
                Id = reservation.Id,
                StartTime = reservation.StartingTime,
                FinishTime = reservation.StartingTime.AddHours(reservation.Duration),
            };
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id)).ReturnsAsync(reservation);

            //Act
            var result = await _controller.Prolong(reservation.Id);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(prolongVM), JsonSerializer.Serialize(model));
            _reservationServiceMock.Verify(x => x.GetById(reservation.Id), Times.Once);
        }

        [Fact]
        public async Task GivenReservationId_WhenPostProlongIsCalled_ThenReturnsNotFoundResult()
        {
            //Arrange
            ProlongVM prolongVM = new()
            {
                Id = reservation.Id,
                ProlongDuration = 1,
                StartTime = reservation.StartingTime,
                FinishTime = reservation.StartingTime.AddHours(reservation.Duration),
            };
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id));

            //Act
            var result = await _controller.Prolong(prolongVM);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _reservationServiceMock.Verify(x => x.GetById(reservation.Id), Times.Once);
        }

        [Fact]
        public async Task GivenReservationId_WhenPostProlongIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            ProlongVM prolongVM = new()
            {
                Id = reservation.Id,
                ProlongDuration = 1,
                StartTime = reservation.StartingTime,
                FinishTime = reservation.StartingTime.AddHours(reservation.Duration),
            };

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id)).ReturnsAsync(reservation);
            _slotServiceMock.Setup(x => x.GetById(parkingSlot.Id)).ReturnsAsync(parkingSlot);

            _slotServiceMock
                .Setup(x => x.IsSlotFreeForReservation(It.IsAny<ParkingSlot>(), It.IsAny<DateTime>(), It.IsAny<uint>()))
                .Returns(true);

            _reservationServiceMock.Setup(x => x.Update(It.IsAny<Reservation>()));

            //Act
            var result = await _controller.Prolong(prolongVM);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.True(_controller.ModelState.IsValid);
            Assert.Equal("Reservation successfully prolonged.", _controller.TempData["SuccessMessage"]);
            Assert.Equal(JsonSerializer.Serialize(prolongVM), JsonSerializer.Serialize(model));

            _reservationServiceMock.Verify(x => x.GetById(reservation.Id), Times.Once);
            _slotServiceMock.Verify(x => x.GetById(reservation.ParkingSlotId), Times.Once);
            _reservationServiceMock.Verify(x => x.Update(It.IsAny<Reservation>()), Times.Once);

            _slotServiceMock.Verify(x => x
                .IsSlotFreeForReservation(It.IsAny<ParkingSlot>(), It.IsAny<DateTime>(), It.IsAny<uint>()), Times.Once);
        }

        [Fact]
        public async Task GivenReservationId_WhenPostProlongIsCalled_ThenReturnsModelErrorIfDurationIsLessThanOne()
        {
            //Arrange
            ProlongVM prolongVM = new()
            {
                Id = reservation.Id,
                StartTime = reservation.StartingTime,
                FinishTime = reservation.StartingTime.AddHours(reservation.Duration),
            };
            _controller.ModelState.AddModelError("ProlongDuration", "Prolong time should be at least 1 hour.");
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id)).ReturnsAsync(reservation);

            //Act
            var result = await _controller.Prolong(prolongVM);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Equal(JsonSerializer.Serialize(prolongVM), JsonSerializer.Serialize(model));
            _reservationServiceMock.Verify(x => x.GetById(reservation.Id), Times.Once);
        }

        [Fact]
        public async Task GivenReservationId_WhenPostProlongIsCalled_ThenReturnsModelErrorIfSlotIsNotFree()
        {
            //Arrange
            ProlongVM prolongVM = new()
            {
                Id = reservation.Id,
                StartTime = reservation.StartingTime,
                FinishTime = reservation.StartingTime.AddHours(reservation.Duration),
            };
            _controller.ModelState.AddModelError("ProlongDuration", "This slot is already reserved for chosen prolong time!");
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id)).ReturnsAsync(reservation);
            _slotServiceMock.Setup(x => x.GetById(parkingSlot.Id)).ReturnsAsync(parkingSlot);
            _slotServiceMock
                .Setup(x => x.IsSlotFreeForReservation(It.IsAny<ParkingSlot>(), It.IsAny<DateTime>(), It.IsAny<uint>()))
                .Returns(true);

            //Act
            var result = await _controller.Prolong(prolongVM);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Equal(JsonSerializer.Serialize(prolongVM), JsonSerializer.Serialize(model));
            _reservationServiceMock.Verify(x => x.GetById(reservation.Id), Times.Once);
            _slotServiceMock.Verify(x => x.GetById(reservation.ParkingSlotId), Times.Once);
            _slotServiceMock.Verify(x => x
                .IsSlotFreeForReservation(It.IsAny<ParkingSlot>(), It.IsAny<DateTime>(), It.IsAny<uint>()), Times.Once);
        }
        #endregion

        #region Delete
        [Fact]
        public async Task GivenReservationId_WhenGetDeleteIsCalled_ThenReturnsNotFoundResult()
        {
            //Arrange
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id));

            //Act
            var result = await _controller.Delete(parkingSlot.Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            _reservationServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
        }

        [Fact]
        public async Task GivenReservationId_WhenGetDeleteIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            _reservationServiceMock
                    .Setup(x => x.GetById(reservation.Id))
                    .ReturnsAsync(reservation);

            //Act
            var result = await _controller.Delete(reservation.Id);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.Equal(JsonSerializer.Serialize(model), JsonSerializer.Serialize(reservation));
            _reservationServiceMock.Verify(x => x.GetById(reservation.Id), Times.Once);
        }

        [Fact]
        public async Task GivenReservationId_WhenPostDeleteIsCalled_ThenReturnsNotFoundResult()
        {
            //Arrange
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id));

            //Act
            var result = await _controller.Delete(reservation.Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            _reservationServiceMock.Verify(x => x.GetById(reservation.Id), Times.Once);
        }

        [Fact]
        public async Task GivenReservationId_WhenPostDeleteIsCalled_ThenReturnsRedirectToActionResult()
        {
            //Arrange
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id)).ReturnsAsync(reservation);
            _reservationServiceMock.Setup(x => x.Remove(reservation));

            //Act
            var result = await _controller.DeleteConfirmed(reservation.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
            _reservationServiceMock.Verify(x => x.GetById(reservation.Id), Times.Once);
            _reservationServiceMock.Verify(x => x.Remove(reservation), Times.Once);
        }
        #endregion
    }
}