using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ParkingZoneApp.Areas.Admin;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ParkingZones;
using System.Text.Json;

namespace ParkingZoneApp.Tests.Controllers.Admin
{
    public class ParkingZoneControllerUnitTests
    {
        private readonly Mock<IParkingZoneService> _parkingZoneServiceMock;
        private readonly ParkingZonesController _controller;
        private readonly ParkingZone? parkingZoneNull = null;
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
            var expectedListOfItems = new List<ParkingZone>()
            {
                new ParkingZone { Id = Guid.NewGuid(), Name = "Parking zone 1"},
                new ParkingZone { Id = Guid.NewGuid(), Name = "Parking zone 2"}
            };

            _parkingZoneServiceMock.Setup(service => service.GetAll()).Returns(expectedListOfItems);

            //Act
            var result = _controller.Index();

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;  
            Assert.IsType<ViewResult>(result);
            Assert.NotEmpty(expectedListOfItems);
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(expectedListOfItems), JsonSerializer.Serialize(model));
            _parkingZoneServiceMock.Verify(x => x.GetAll(), Times.Once);
            _parkingZoneServiceMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Details
        [Fact]
        public void GivenValidId_WhenDetailsIsCalled_ThenReturnViewResult()
        {
            //Arrrange 
            var parkingZoneID = Guid.NewGuid();
            var parkingZone = new ParkingZone()
            {
                Id = parkingZoneID,
                Name = "Test Name",
                Address = "Test Address",
                CreatedDate = new DateOnly(2024, 1, 1),
            };

            _parkingZoneServiceMock.Setup(service => service.GetById(parkingZoneID)).Returns(parkingZone);

            //Act
            var result = _controller.Details(parkingZoneID) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<DetailsVM>(result.Model);
            Assert.Equal(JsonSerializer.Serialize(parkingZone), JsonSerializer.Serialize(result.Model));
        }

        [Fact]
        public void GivenInvalidId_WhenDetailsIsCalled_ThenReturnNotFound()
        {
            //Arrange
            var parkingZoneID = Guid.NewGuid();
            _parkingZoneServiceMock.Setup(service => service.GetById(parkingZoneID)).Returns(parkingZoneNull);

            //Act
            var result = _controller.Details(parkingZoneID);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region Create
        [Fact]
        public void GivenValidCreateVM_WhenCreateIsCalled_ThenReturnsRedirectToIndex()
        {
            //Arrange
            var createVM = new CreateVM();

            //Act
            var result = _controller.Create(createVM) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void GivenInvalidCreateVM_WhenCreateIsCalled_ReturnsViewResult()
        {
            //Arrange
            var createVM = new CreateVM();

            //Act
            var result = _controller.Create(createVM);

            //Assert
            Assert.NotNull(result);
            _parkingZoneServiceMock.Verify(service => service.Insert(It.IsAny<ParkingZone>()), Times.Once);
        }
        #endregion

        #region Edit
        [Fact]
        public void GivenValidIDAndEditVM_WhenEditIsCalled_ThenReturnsRedirectToIndex()
        {
            //Arrange
            var id = Guid.NewGuid();
            var editVM = new EditVM();
            var parkingZone = new ParkingZone();
            parkingZone.Id = id;
            _parkingZoneServiceMock.Setup(x => x.GetById(id)).Returns(parkingZone);

            //Act
            var result = _controller.Edit(id, editVM) as RedirectToActionResult;

            //Assert
            _parkingZoneServiceMock.Verify(service => service.Update(parkingZone), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void GivenValidId_WhenEditIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            var parkingZoneID = Guid.NewGuid();
            var parkingZone = new ParkingZone()
            {
                Id = parkingZoneID,
                Name = "Test Name",
                Address = "Test Address",
                CreatedDate = new DateOnly(),
            };
            _parkingZoneServiceMock.Setup(service => service.Update(parkingZone)).Throws(new DbUpdateConcurrencyException());
            _parkingZoneServiceMock.Setup(service => service.GetById(parkingZoneID)).Returns(parkingZone);

            //Act
            var result = _controller.Edit(parkingZoneID) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<EditVM>(result.Model);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(JsonSerializer.Serialize(parkingZone), JsonSerializer.Serialize(result.Model));
        }

        [Fact]
        public void GivenInvalidId_WhenEditIsCalled_ThenReturnsNotFound()
        {
            //given
            var parkingZoneId = Guid.NewGuid();
            _parkingZoneServiceMock.Setup(service => service.GetById(parkingZoneId)).Returns(parkingZoneNull);

            //When
            var result = _controller.Edit(parkingZoneId);

            //Then
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region Delete
        [Fact]
        public void GivenValidId_WhenDeleteIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            var parkingZoneID = Guid.NewGuid();
            var parkingZone = new ParkingZone()
            {
                Id = parkingZoneID,
                Name = "Test Name",
                Address = "Test Address",
                CreatedDate = new DateOnly(2024, 1, 1),
            };

            _parkingZoneServiceMock.Setup(service => service.GetById(parkingZoneID)).Returns(parkingZone);

            //Act
            var result = _controller.Delete(parkingZoneID) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ParkingZone>(result.Model);
            Assert.Equal(parkingZone.Id, ((ParkingZone)result.Model).Id);
            Assert.Equal(parkingZone.Name, ((ParkingZone)result.Model).Name);
            Assert.Equal(parkingZone.Address, ((ParkingZone)result.Model).Address);
            Assert.Equal(parkingZone.CreatedDate, ((ParkingZone)result.Model).CreatedDate);
        }

        [Fact]
        public void GivenInvalidId_WhenDeleteIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            var parkingZoneID = Guid.NewGuid();
            _parkingZoneServiceMock.Setup(service => service.GetById(parkingZoneID)).Returns(parkingZoneNull);

            //Act
            var result = _controller.Delete(parkingZoneID);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GivenValidInputs_WhenDeleteConfirmedIsCalled_ThenReturnsRedirectToIndex()
        {
            //Arrange
            var id = Guid.NewGuid();
            var parkingZone = new ParkingZone();
            parkingZone.Id = id;
            _parkingZoneServiceMock.Setup(x => x.GetById(id)).Returns(parkingZone);

            //Act
            var result = _controller.DeleteConfirmed(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
        #endregion
    }
}