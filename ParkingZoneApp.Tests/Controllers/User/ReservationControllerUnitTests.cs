using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingZoneApp.Areas.User.Controllers;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Services.Interfaces;
using System.Security.Claims;

namespace ParkingZoneApp.Tests.Controllers.User
{
    public class ReservationControllerUnitTests
    {
        private readonly Mock<IReservationService> _reservationServiceMock;
        private readonly Mock<IParkingSlotService> _slotServiceMock;
        private readonly Mock<IParkingZoneService> _zoneServiceMock;
        private readonly ReservationController _controller;
        private static readonly Reservation reservation = new()
        {
            Id = Guid.NewGuid(),
            Duration = 2,
            StartingTime = DateTime.UtcNow,
            ParkingSlotId = Guid.NewGuid(),
            ParkingZoneId = Guid.NewGuid(),
            VehicleNumber = "DDD777",
            UserId = "UserId"
        };
        public static readonly IEnumerable<Reservation> reservations = [reservation];

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

            _reservationServiceMock.Setup(x => x.GetReservationsByUser(It.IsAny<string>())).Returns(reservations);
            //Act
            var result = _controller.Index();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            _reservationServiceMock.Verify((x => x.GetReservationsByUser(It.IsAny<string>())), Times.Once);
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
    }
}