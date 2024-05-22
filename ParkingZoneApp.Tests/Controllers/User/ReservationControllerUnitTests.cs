using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Nest;
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
        private readonly Mock<IParkingZoneService> _zoneServiceMock;
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

        public ReservationControllerUnitTests()
        {
            _reservationServiceMock = new Mock<IReservationService>();
            _slotServiceMock = new Mock<IParkingSlotService>();
            _zoneServiceMock = new Mock<IParkingZoneService>();
            _controller = new ReservationController(_reservationServiceMock.Object, _zoneServiceMock.Object, _slotServiceMock.Object);
        }

        #region Index
        [Fact]
        public void GivenNothing_WhenGetIndexIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            var mockClaimsPrincipal = CreateMockClaimsPrincipal();

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockClaimsPrincipal }
            };

            _reservationServiceMock.Setup(x => x.GetReservationsByUserId(It.IsAny<string>())).Returns(reservations);
            //Act
            var result = _controller.Index();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            _reservationServiceMock.Verify((x => x.GetReservationsByUserId(It.IsAny<string>())), Times.Once);
        }

        [Fact]
        public void GivenNothing_WhenGetIndexIsCalled_ThenNotFoundResult()
        {
            //Arrange
            reservation.UserId = null;
            _controller.ControllerContext = new();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal()
            };

            //Act
            var result = _controller.Index();

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
        public void GivenReservationId_WhenGetProlongIsCalled_ThenReturnsNotFoundResult()
        {
            //Arrange
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id));

            //Act
            var result = _controller.Prolong(reservation.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _reservationServiceMock.Verify(x => x.GetById(reservation.Id), Times.Once);
        }

        [Fact]
        public void GivenReservationId_WhenGetProlongIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            ProlongVM prolongVM = new()
            {
                Id = reservation.Id,
                StartTime = reservation.StartingTime,
                FinishTime = reservation.StartingTime.AddHours(reservation.Duration),
            };
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id)).Returns(reservation);

            //Act
            var result = _controller.Prolong(reservation.Id);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(prolongVM), JsonSerializer.Serialize(model));
            _reservationServiceMock.Verify(x => x.GetById(reservation.Id), Times.Once);
        }

        [Fact]
        public void GivenReservationId_WhenPostProlongIsCalled_ThenReturnsNotFoundResult()
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
            var result = _controller.Prolong(prolongVM);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _reservationServiceMock.Verify(x => x.GetById(reservation.Id), Times.Once);
        }

        [Fact]
        public void GivenReservationId_WhenPostProlongIsCalled_ThenReturnsViewResult()
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
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id)).Returns(reservation);
            _slotServiceMock.Setup(x => x.GetById(parkingSlot.Id)).Returns(parkingSlot);

            _slotServiceMock
                .Setup(x => x.IsSlotFreeForReservation(It.IsAny<ParkingSlot>(), It.IsAny<DateTime>(), It.IsAny<uint>()))
                .Returns(true);

            _reservationServiceMock.Setup(x => x.Update(It.IsAny<Reservation>()));

            //Act
            var result = _controller.Prolong(prolongVM);

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
        public void GivenReservationId_WhenPostProlongIsCalled_ThenReturnsModelErrorIfDurationIsLessThanOne()
        {
            //Arrange
            ProlongVM prolongVM = new()
            {
                Id = reservation.Id,
                StartTime = reservation.StartingTime,
                FinishTime = reservation.StartingTime.AddHours(reservation.Duration),
            };
            _controller.ModelState.AddModelError("ProlongDuration", "Prolong time should be at least 1 hour.");
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id)).Returns(reservation);

            //Act
            var result = _controller.Prolong(prolongVM);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Equal(JsonSerializer.Serialize(prolongVM), JsonSerializer.Serialize(model));
            _reservationServiceMock.Verify(x => x.GetById(reservation.Id), Times.Once);
        }

        [Fact]
        public void GivenReservationId_WhenPostProlongIsCalled_ThenReturnsModelErrorIfSlotIsNotFree()
        {
            //Arrange
            ProlongVM prolongVM = new()
            {
                Id = reservation.Id,
                StartTime = reservation.StartingTime,
                FinishTime = reservation.StartingTime.AddHours(reservation.Duration),
            };
            _controller.ModelState.AddModelError("ProlongDuration", "This slot is already reserved for chosen prolong time!");
            _reservationServiceMock.Setup(x => x.GetById(reservation.Id)).Returns(reservation);
            _slotServiceMock.Setup(x => x.GetById(parkingSlot.Id)).Returns(parkingSlot);
            _slotServiceMock
                .Setup(x => x.IsSlotFreeForReservation(It.IsAny<ParkingSlot>(), It.IsAny<DateTime>(), It.IsAny<uint>()))
                .Returns(true);

            //Act
            var result = _controller.Prolong(prolongVM);

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
    }
}