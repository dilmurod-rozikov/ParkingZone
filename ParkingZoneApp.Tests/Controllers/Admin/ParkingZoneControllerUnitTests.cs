using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ParkingZoneApp.Areas.Admin;
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
        private readonly ParkingZonesController _controller;
        private readonly ParkingZone parkingZone = new()
        {
            Id = Guid.NewGuid(),
            Name = "Test Name",
            Address = "Test Address",
            ParkingSlots = [ new() ]
        };

        public ParkingZoneControllerUnitTests()
        {
            _parkingZoneServiceMock = new Mock<IParkingZoneService>();
            _controller = new ParkingZonesController(_parkingZoneServiceMock.Object);
        }

        #region Index
        [Fact]
        public void GivenNothing_WhenIndexIsCalled_ThenReturnsViewResult()
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
                    .Returns(testZones);

            //Act
            var result = _controller.Index();

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(expectedListOfItems), JsonSerializer.Serialize(model));
            _parkingZoneServiceMock.Verify(x => x.GetAll(), Times.Once);
            _parkingZoneServiceMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Details
        [Fact]
        public void GivenValidParkingZoneId_WhenGetDetailsIsCalled_ThenReturnViewResult()
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
                    .Returns(parkingZone);

            //Act
            var result = _controller.Details(parkingZone.Id);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(detailsVM), JsonSerializer.Serialize(model));
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
        }

        [Fact]
        public void GivenInvalidParkingZoneId_WhenGetDetailsIsCalled_ThenReturnNotFound()
        {
            //Arrange
            _parkingZoneServiceMock
                    .Setup(service => service.GetById(parkingZone.Id));

            //Act
            var result = _controller.Details(parkingZone.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
        }
        #endregion

        #region Create
        [Fact]
        public void GivenValidCreateVM_WhenPostCreateIsCalled_ThenModelStateIsTrueAndReturnsRedirectToIndex()
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
            var result = _controller.Create(createVM);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
            Assert.True(_controller.ModelState.IsValid);
            _parkingZoneServiceMock.Verify(service => service.Insert(It.IsAny<ParkingZone>()), Times.Once);
        }

        [Fact]
        public void GivenInvalidCreateVM_WhenPostCreateIsCalled_ThenModelStateIsFalseAndReturnsViewResult()
        {
            //Arrange
            CreateVM createVM = new();
            _controller.ModelState.AddModelError("id", "id is required");
    
            //Act
            var result = _controller.Create(createVM);

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
        public void GivenValidIDAndEditVM_WhenPostEditIsCalled_ThenModelStateIsTrueReturnsRedirectToIndex()
        {
            //Arrange
            EditVM editVM = new(parkingZone)
            {
                Address = null
            };

            _parkingZoneServiceMock
                    .Setup(x => x.GetById(parkingZone.Id))
                    .Returns(parkingZone);

            _parkingZoneServiceMock
                    .Setup(x => x.Update(parkingZone));

            //Act
            var result = _controller.Edit(parkingZone.Id, editVM);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(_controller.ModelState.IsValid);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
            _parkingZoneServiceMock.Verify(x => x.Update(parkingZone), Times.Once);
        }

        [Fact]
        public void GivenValidParkingZoneId_WhenPostEditIsCalled_ThenModelStateIsTrueReturnDbUpdateConcurrencyException()
        {
            //Arrange
            EditVM editVM = new EditVM(parkingZone);
            _parkingZoneServiceMock
                    .Setup(x => x.Update(parkingZone))
                    .Throws<DbUpdateConcurrencyException>();

            //Act
            var result = _controller.Edit(parkingZone.Id, editVM);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
            _parkingZoneServiceMock.Verify(x => x.Update(parkingZone), Times.Never);
        }

        [Fact]
        public void GivenValidParkingZoneId_WhenEditIsCalled_ThenModelStateIsFalseReturnsViewResult()
        {
            //Arrange
            _controller.ModelState.AddModelError("field", "property is invalid");
            _parkingZoneServiceMock
                    .Setup(service => service.GetById(parkingZone.Id))
                    .Returns(parkingZone);

            //Act
            var result = _controller.Edit(parkingZone.Id);

            //Assert
            Assert.IsType<ViewResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
        }

        [Fact]
        public void GivenInvalidParkingZoneId_WhenEditIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _parkingZoneServiceMock
                    .Setup(service => service.GetById(parkingZone.Id));

            //Act
            var result = _controller.Edit(parkingZone.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
        }
        #endregion

        #region Delete
        [Fact]
        public void GivenValidParkingZoneId_WhenGetDeleteIsCalled_ThenReturnsViewResult()
        {
            //Arrange           
            _parkingZoneServiceMock
                    .Setup(service => service.GetById(parkingZone.Id))
                    .Returns(parkingZone);

            //Act
            var result = _controller.Delete(parkingZone.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
        }

        [Fact]
        public void GivenInvalidParkingZoneId_WhenGetDeleteIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _parkingZoneServiceMock
                    .Setup(service => service.GetById(parkingZone.Id));

            //Act
            var result = _controller.Delete(parkingZone.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once);
        }

        [Fact]
        public void GivenValidParkingZoneId_WhenPostDeleteConfirmedIsCalled_ThenReturnsRedirectToIndex()
        {
            //Arrange
            _parkingZoneServiceMock
                    .Setup(x => x.GetById(parkingZone.Id))
                    .Returns(parkingZone);

            _parkingZoneServiceMock
                    .Setup(x => x.Remove(parkingZone));

            //Act
            var result = _controller.DeleteConfirmed(parkingZone.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
            _parkingZoneServiceMock.Verify(x => x.GetById(parkingZone.Id), Times.Once());
            _parkingZoneServiceMock.Verify(x => x.Remove(parkingZone), Times.Once());
        }
        #endregion
    }
}