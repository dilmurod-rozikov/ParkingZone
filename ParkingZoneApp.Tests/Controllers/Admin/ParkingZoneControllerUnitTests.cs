using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Protocol;
using ParkingZoneApp.Areas.Admin;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ParkingZoneVMs;
using System.Text.Json;

namespace ParkingZoneApp.Tests.Controllers.Admin
{
    public class ParkingZoneControllerUnitTests
    {
        private readonly Mock<IParkingZoneService> _parkingZoneServiceMock;
        private readonly ParkingZoneController _controller;
        private static readonly Guid parkingZoneId = Guid.NewGuid();
        private static readonly Guid parkingSlotId = Guid.NewGuid();
        private static readonly Reservation reservation = new()
        {
            Id = Guid.NewGuid(),
            Duration = 1,
            StartingTime = DateTime.Now.AddHours(-10),
            ParkingSlotId = parkingSlotId,
            ParkingSlot = new(),
            ParkingZoneId = parkingZoneId,
            UserId = "User-test-id",
            VehicleNumber = "Number",
        };
        private static readonly List<Reservation> reservations = [reservation];
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
                    Reservations = reservations,
                    IsAvailable = false,
                    Number = 1,
                },
            ]
        };

        public ParkingZoneControllerUnitTests()
        {
            _parkingZoneServiceMock = new Mock<IParkingZoneService>();
            _controller = new ParkingZoneController(_parkingZoneServiceMock.Object);
        }

        #region Index
        [Fact]
        public async Task GivenNothing_WhenIndexIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            var testZones = new List<ParkingZone>()
            {
                parkingZone
            };
            var expectedListOfItems = new List<ListItemVM>()
            {
                new(parkingZone)
            };

            _parkingZoneServiceMock
                    .Setup(service => service.GetAll())
                    .ReturnsAsync(testZones);

            //Act
            var result = await _controller.Index();

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(expectedListOfItems), JsonSerializer.Serialize(model));
            _parkingZoneServiceMock.Verify(x => x.GetAll(), Times.Once);
            _parkingZoneServiceMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetCurrentCars
        [Fact]
        public async Task GivenZoneId_WhenGetCurrentCarsIsCalled_ThenReturnsNotFoundResult()
        {
            //Arrange
            Mock<IReservationService> _reservationServiceMock = new();
            _parkingZoneServiceMock.Setup(x => x.GetById(It.IsAny<Guid>()));

            //Act
            var result = await _controller.GetCurrentCars(parkingZone.Id, _reservationServiceMock.Object);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _parkingZoneServiceMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GivenZoneId_WhenGetCurrentCarsIsCalled_ThenReturnsListOfCurrentCars()
        {
            //Arrange
            Mock<IReservationService> _reservationServiceMock = new();
            _parkingZoneServiceMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(parkingZone);
            _reservationServiceMock.Setup(x => x.GetReservationsByZoneId(It.IsAny<Guid>())).ReturnsAsync(reservations);
            GetCurrentCarsVM vm =
                new()
                {
                    VehicleNumber = reservation.VehicleNumber,
                    Number = reservation.ParkingSlot.Number,
                };
            //Act
            var result = await _controller.GetCurrentCars(parkingZone.Id, _reservationServiceMock.Object);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(model), JsonSerializer.Serialize(new List<GetCurrentCarsVM>() { vm }));
            _parkingZoneServiceMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _reservationServiceMock.Verify(x => x.GetReservationsByZoneId(It.IsAny<Guid>()), Times.Once);
        }
        #endregion

        #region Details
        [Fact]
        public async Task GivenValidParkingZoneId_WhenGetDetailsIsCalled_ThenReturnViewResult()
        {
            //Arrange 
            DetailsVM detailsVM = new()
            {
                Id = parkingZone.Id,
                Name = parkingZone.Name,
                Address = parkingZone.Address,
                CreatedDate = parkingZone.CreatedDate
            };

            _parkingZoneServiceMock
                    .Setup(service => service.GetById(parkingZone.Id))
                    .ReturnsAsync(parkingZone);

            //Act
            var result = await _controller.Details(parkingZone.Id);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(detailsVM), JsonSerializer.Serialize(model));
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
        }

        [Fact]
        public async Task GivenInvalidParkingZoneId_WhenGetDetailsIsCalled_ThenReturnNotFound()
        {
            //Arrange
            _parkingZoneServiceMock
                    .Setup(service => service.GetById(parkingZone.Id));

            //Act
            var result = await _controller.Details(parkingZone.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
        }
        #endregion

        #region Create
        [Fact]
        public async Task GivenValidCreateVM_WhenPostCreateIsCalled_ThenModelStateIsTrueAndReturnsRedirectToIndex()
        {
            //Arrange
            CreateVM createVM = new()
            {
                Name = "Test Name",
                Address = "Test Address",
            };

            _parkingZoneServiceMock
                    .Setup(x => x.Insert(parkingZone));

            //Act
            var result = await _controller.Create(createVM);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
            Assert.True(_controller.ModelState.IsValid);
            _parkingZoneServiceMock.Verify(service => service.Insert(It.IsAny<ParkingZone>()), Times.Once);
        }

        [Fact]
        public async Task GivenInvalidCreateVM_WhenPostCreateIsCalled_ThenModelStateIsFalseAndReturnsViewResult()
        {
            //Arrange
            CreateVM createVM = new();
            _controller.ModelState.AddModelError("id", "id is required");

            //Act
            var result = await _controller.Create(createVM);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsAssignableFrom<CreateVM>(model);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Equal(JsonSerializer.Serialize(createVM), JsonSerializer.Serialize(model));
        }

        [Fact]
        public void GivenNothing_WhenGetCreateIsCalled_ReturnsViewResult()
        {
            //Arrange

            //Act
            var result = _controller.Create();

            //Assert
            var model = Assert.IsType<ViewResult>(result);
            Assert.Null(model.ViewName);
        }
        #endregion

        #region Edit
        [Fact]
        public async Task GivenValidIDAndEditVM_WhenPostEditIsCalled_ThenModelStateIsTrueReturnsRedirectToIndex()
        {
            //Arrange
            EditVM editVM = new(parkingZone)
            {
                Address = null
            };

            _parkingZoneServiceMock
                    .Setup(x => x.GetById(parkingZone.Id))
                    .ReturnsAsync(parkingZone);

            _parkingZoneServiceMock
                    .Setup(x => x.Update(parkingZone));

            //Act
            var result = await _controller.Edit(parkingZone.Id, editVM);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(_controller.ModelState.IsValid);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
            _parkingZoneServiceMock.Verify(x => x.Update(parkingZone), Times.Once);
        }

        [Fact]
        public async Task GivenValidParkingZoneId_WhenPostEditIsCalled_ThenModelStateIsTrueReturnDbUpdateConcurrencyException()
        {
            //Arrange
            var editVM = new EditVM(parkingZone);
            _parkingZoneServiceMock
                    .Setup(x => x.Update(parkingZone))
                    .Throws<DbUpdateConcurrencyException>();

            //Act
            var result = await _controller.Edit(parkingZone.Id, editVM);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
            _parkingZoneServiceMock.Verify(x => x.Update(parkingZone), Times.Never);
        }

        [Fact]
        public async Task GivenValidParkingZoneId_WhenEditIsCalled_ThenModelStateIsFalseReturnsViewResult()
        {
            //Arrange
            _controller.ModelState.AddModelError("field", "property is invalid");
            _parkingZoneServiceMock
                    .Setup(service => service.GetById(parkingZone.Id))
                    .ReturnsAsync(parkingZone);

            //Act
            var result = await _controller.Edit(parkingZone.Id);

            //Assert
            Assert.IsType<ViewResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
        }

        [Fact]
        public async Task GivenInvalidParkingZoneId_WhenEditIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _parkingZoneServiceMock
                    .Setup(service => service.GetById(parkingZone.Id));

            //Act
            var result = await _controller.Edit(parkingZone.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
        }
        #endregion

        #region Delete
        [Fact]
        public async Task GivenValidParkingZoneId_WhenGetDeleteIsCalled_ThenReturnsViewResult()
        {
            //Arrange           
            _parkingZoneServiceMock
                    .Setup(service => service.GetById(parkingZone.Id))
                    .ReturnsAsync(parkingZone);

            //Act
            var result = await _controller.Delete(parkingZone.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
        }

        [Fact]
        public async Task GivenInvalidParkingZoneId_WhenGetDeleteIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _parkingZoneServiceMock
                    .Setup(service => service.GetById(parkingZone.Id));

            //Act
            var result = await _controller.Delete(parkingZone.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
        }

        [Fact]
        public async Task GivenValidParkingZoneId_WhenPostDeleteConfirmedIsCalled_ThenReturnsRedirectToIndex()
        {
            //Arrange
            _parkingZoneServiceMock
                    .Setup(x => x.GetById(parkingZone.Id))
                    .ReturnsAsync(parkingZone);

            _parkingZoneServiceMock
                    .Setup(x => x.Remove(parkingZone));

            //Act
            var result = await _controller.DeleteConfirmed(parkingZone.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once());
            _parkingZoneServiceMock.Verify(x => x.Remove(parkingZone), Times.Once());
        }
        #endregion

        #region FilterByPeriod
        [Fact]
        public async Task GivenZoneIdAndPeriodRange_WhenFilterByPeriodIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _parkingZoneServiceMock.Setup(x => x.GetById(It.IsAny<Guid>()));

            //Act
            var result = await _controller.FilterByPeriod(parkingZoneId, PeriodRange.Last7Days);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _parkingZoneServiceMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        }
        [Fact]
        public async Task GivenZoneIdAndPeriodRange_WhenFilterByPeriodIsCalled_ThenReturnsTotalCategoryUsage()
        {
            //Arrange
            var dic = new Dictionary<SlotCategory, long>
            {
                { SlotCategory.Business, 1}
            };
            var myObj = new { categoryHours = dic };
            _parkingZoneServiceMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(parkingZone);
            _parkingZoneServiceMock.Setup(x => x.FilterByPeriodOnSlotCategory(parkingZone, PeriodRange.Last7Days)).Returns(dic);

            //Act
            var result = await _controller.FilterByPeriod(parkingZoneId, PeriodRange.Last7Days) as JsonResult;

            //Assert
            Assert.NotNull(result);
            var data = Assert.IsType<JsonResult>(result).Value;
            Assert.Equal(JsonSerializer.Serialize(myObj), JsonSerializer.Serialize(data));
            _parkingZoneServiceMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _parkingZoneServiceMock.Verify(x => x.FilterByPeriodOnSlotCategory(parkingZone, PeriodRange.Last7Days), Times.Once);
        }

        #endregion
    }
}