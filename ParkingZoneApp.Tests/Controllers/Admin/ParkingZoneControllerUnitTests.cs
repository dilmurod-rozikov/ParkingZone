using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ParkingZoneApp.Areas.Admin;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ParkingZones;

namespace ParkingZoneApp.Tests.Controllers.Admin
{
    public class ParkingZoneControllerUnitTests
    {
        private readonly Mock<IParkingZoneService> _parkingZoneServiceMock;
        private readonly ParkingZonesController _controller;

        public ParkingZoneControllerUnitTests()
        {
            _parkingZoneServiceMock = new Mock<IParkingZoneService>();
            _controller = new ParkingZonesController(_parkingZoneServiceMock.Object);
        }

        #region Index
        [Fact]
        public void GivenListOfParkingZones_WhenIndexCalled_ReturnViewResult()
        {
            //Given
            var listOfItems = new List<ParkingZone>()
            {
                new ParkingZone { Id = Guid.NewGuid()},
                new ParkingZone { Id = Guid.NewGuid()}
            };

            _parkingZoneServiceMock.Setup(service => service.GetAll()).Returns(listOfItems);

            //When
            var result = _controller.Index() as ViewResult;

            //Then
            Assert.IsType<ViewResult>(result);
            Assert.NotEmpty(listOfItems);
        }
        #endregion

        #region Details
        [Fact]
        public void GivenValidId_WhenDetailsIsCalled_ThenReturnViewResult()
        {
            //Given 
            var parkingZoneID = Guid.NewGuid();
            var parkingZone = new ParkingZone()
            {
                Id = parkingZoneID,
                Name = "Test Name",
                Address = "Test Address",
                CreatedDate = new DateOnly(2024, 1, 1),
            };

            _parkingZoneServiceMock.Setup(service => service.GetById(parkingZoneID)).Returns(parkingZone);

            //When
            var result = _controller.Details(parkingZoneID) as ViewResult;

            //Then
            Assert.NotNull(result);
            Assert.IsType<DetailsVM>(result.Model);
            Assert.Equal(parkingZone.Id, ((DetailsVM)result.Model).Id);
            Assert.Equal(parkingZone.Name, ((DetailsVM)result.Model).Name);
            Assert.Equal(parkingZone.Address, ((DetailsVM)result.Model).Address);
            Assert.Equal(parkingZone.CreatedDate, ((DetailsVM)result.Model).CreatedDate);
        }

        [Fact]
        public void GivenInvalidId_WhenDetailsIsCalled_ThenReturnNotFound()
        {
            //Given
            var parkingZoneID = Guid.NewGuid();
            _parkingZoneServiceMock.Setup(service => service.GetById(parkingZoneID)).Returns((ParkingZone)null);

            //When
            var result = _controller.Details(parkingZoneID);

            //Then
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region Create
        [Fact]
        public void GivenValidInputs_WhenCreateCalled_ReturnRedirectToIndex()
        {
            //Given
            var createVM = new CreateVM();

            //When
            var result = _controller.Create(createVM) as RedirectToActionResult;

            //Then
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void GivenValidInputs_WhenCreateCallsInsert_ReturnsViewResult()
        {
            //Given
            var createVM = new CreateVM();

            //When
            var result = _controller.Create(createVM);

            //Then
            _parkingZoneServiceMock.Verify(service => service.Insert(It.IsAny<ParkingZone>()), Times.Once);
        }

        #endregion

        #region Edit
        [Fact]
        public void GivenValidInputs_WhenEditIsCalled_ReturnRedirectToIndex()
        {
            //Given
            var id = Guid.NewGuid();
            var editVM = new EditVM();
            var parkingZone = new ParkingZone();
            parkingZone.Id = id;
            _parkingZoneServiceMock.Setup(x => x.GetById(id)).Returns(parkingZone);

            //When
            var result = _controller.Edit(id, editVM) as RedirectToActionResult;

            //Then
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void GivenInvalidId_WhenEditIsCalled_ReturnNotFound()
        {
            //given
            var parkingZoneId = Guid.NewGuid();
            _parkingZoneServiceMock.Setup(service => service.GetById(parkingZoneId)).Returns((ParkingZone)null);

            //When
            var result = _controller.Edit(parkingZoneId);

            //Then
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GivenValidId_WhenEditIsCalled_ReturnViewResult()
        {
            //Given
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

            //When
            var result = _controller.Edit(parkingZoneID) as ViewResult;

            //Then
            Assert.NotNull(result);
            Assert.IsType<EditVM>(result.Model);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(parkingZone.Name, ((EditVM)result.Model).Name);
            Assert.Equal(parkingZone.Id, ((EditVM)result.Model).Id);
            Assert.Equal(parkingZone.Address, ((EditVM)result.Model).Address);
            Assert.Equal(parkingZone.CreatedDate, ((EditVM)result.Model).CreatedDate);
        }
        #endregion

        #region Delete
        [Fact]
        public void GivenValidId_WhenDeleteIsCalled_ThenReturnViewResult()
        {
            var parkingZoneID = Guid.NewGuid();
            var parkingZone = new ParkingZone()
            {
                Id = parkingZoneID,
                Name = "Test Name",
                Address = "Test Address",
                CreatedDate = new DateOnly(2024, 1, 1),
            };

            _parkingZoneServiceMock.Setup(service => service.GetById(parkingZoneID)).Returns(parkingZone);

            //When
            var result = _controller.Delete(parkingZoneID) as ViewResult;

            //Then
            Assert.NotNull(result);
            Assert.IsType<ParkingZone>(result.Model);
            Assert.Equal(parkingZone.Id, ((ParkingZone)result.Model).Id);
            Assert.Equal(parkingZone.Name, ((ParkingZone)result.Model).Name);
            Assert.Equal(parkingZone.Address, ((ParkingZone)result.Model).Address);
            Assert.Equal(parkingZone.CreatedDate, ((ParkingZone)result.Model).CreatedDate);
        }

        [Fact]
        public void GivenInvalidId_WhenDeleteIsCalled_ThenReturnNotFound()
        {
            //Given
            var parkingZoneID = Guid.NewGuid();
            _parkingZoneServiceMock.Setup(service => service.GetById(parkingZoneID)).Returns((ParkingZone)null);

            //When
            var result = _controller.Delete(parkingZoneID);

            //Then
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GivenValidInputs_WhenDeleteIsCalled_ReturnRedirectToIndex()
        {
            //Given
            var id = Guid.NewGuid();
            var parkingZone = new ParkingZone();
            parkingZone.Id = id;
            _parkingZoneServiceMock.Setup(x => x.GetById(id)).Returns(parkingZone);

            //When
            var result = _controller.DeleteConfirmed(id) as RedirectToActionResult;

            //Then
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
        #endregion


    }
}