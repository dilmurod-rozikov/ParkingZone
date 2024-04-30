﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly Mock<IParkingZoneService> _parkingZoneServiceMock;
        private readonly ParkingSlotController _controller;
        private static readonly ParkingZone parkingZone = new()
        {
            Id = Guid.NewGuid(),
            Name = "name",
            Address = "address",
            CreatedDate = new DateOnly(),
            ParkingSlots = [ new() ]
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
            _parkingSlotServiceMock = new Mock<IParkingSlotService>();
            _parkingZoneServiceMock = new Mock<IParkingZoneService>();
            _controller = new ParkingSlotController(_parkingSlotServiceMock.Object, _parkingZoneServiceMock.Object);
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
                new(parkingSlot) { }
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

        #region Create
        [Fact]
        public void GivenParkingZoneId_WhenCreateGetIsCalled_ThenReturnViewResult()
        {
            //Arrange
            CreateVM expectedCreateVM = new()
            {
                ParkingZoneId = parkingZone.Id,
            };

            //Act
            var result = _controller.Create(expectedCreateVM.ParkingZoneId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(expectedCreateVM.ParkingZoneId), JsonSerializer.Serialize(parkingZone.Id));
        }

        [Fact]
        public void GivenCreateVM_WhenCreatePostIsCalled_ThenReturnModelError()
        {
            //Arrange
            CreateVM expectedCreateVM = new()
            {
                Number = parkingSlot.Number,
                Category = parkingSlot.Category,
                ParkingZoneId = parkingSlot.ParkingZoneId,
                IsAvailable = parkingSlot.IsAvailable
            };

            _parkingSlotServiceMock
                    .Setup(x => x.IsUniqueNumber(expectedCreateVM.ParkingZoneId, expectedCreateVM.Number))
                    .Returns(false);

            //Act
            var result = _controller.Create(expectedCreateVM);

            //Assert
            Assert.NotNull(result);
            _parkingSlotServiceMock
                    .Verify(x => x.IsUniqueNumber(parkingZone.Id, parkingSlot.Number), Times.Once);
        }

        [Fact]
        public void GivenCreateVM_WhenCreatePostIsCalled_ThenModelStateIsFalseAndReturnsViewResult()
        {
            //Arrange
            CreateVM expectedCreateVM = new();
            _controller.ModelState.AddModelError("Number", "Number is required");

            //Act
            var result = _controller.Create(expectedCreateVM);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsAssignableFrom<CreateVM>(model);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Equal(JsonSerializer.Serialize(expectedCreateVM), JsonSerializer.Serialize(model));
        }

        [Fact]
        public void GivenCreateVM_WhenCreatePostIsCalled_ThenModelStateIsTrueAndReturnsRedirectToIndex()
        {
            //Arrange
            CreateVM expectedCreateVM = new()
            {
                Number = parkingSlot.Number,
                Category = parkingSlot.Category,
                IsAvailable = parkingSlot.IsAvailable,
                ParkingZoneId = parkingSlot.ParkingZoneId,
            };

            _parkingSlotServiceMock
                    .Setup(x => x.Insert(parkingSlot));

            //Act
            var result = _controller.Create(expectedCreateVM);

            //Assert
            Assert.NotNull(result);
            Assert.True(_controller.ModelState.IsValid);
            _parkingSlotServiceMock.Verify(x => x.Insert(It.IsAny<ParkingSlot>()), Times.Once);
        }
        #endregion

        #region Edit
        [Fact]
        public void GivenParkingSlotId_WhenEditGetIsCalled_ThenReturnViewResult()
        {
            //Arrange
            EditVM expectedEditVM = new(parkingSlot);
            _parkingSlotServiceMock.Setup(x => x.GetById(parkingSlot.Id)).Returns(parkingSlot);

            //Act
            var result = _controller.Edit(parkingSlot.Id);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(model), JsonSerializer.Serialize(expectedEditVM));
            _parkingSlotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
        }

        [Fact]
        public void GivenParkingSlotId_WhenEditGetIsCalled_ThenReturnNotFound()
        {
            //Arrange
            _parkingSlotServiceMock.Setup(x => x.GetById(parkingSlot.Id));

            //Act
            var result = _controller.Edit(parkingSlot.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _parkingSlotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
        }

        [Fact]
        public void GivenEditVMAndParkingSlotId_WhenEditPostIsCalled_ThenReturnNotFound()
        {
            //Arrange
            _parkingSlotServiceMock.Setup(x => x.GetById(parkingSlot.Id));

            //Act
            var result = _controller.Edit(parkingSlot.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _parkingSlotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
        }

        [Fact]
        public void GivenEditVMAndParkingSlotId_WhenEditPostIsCalled_ThenReturnModelError()
        {
            //Arrange
            EditVM editVM = new(parkingSlot);
            _controller.ModelState.AddModelError("Number", "Number is not valid");
            _parkingSlotServiceMock
                    .Setup(x => x.GetById(parkingSlot.Id))
                    .Returns(parkingSlot);
            _parkingSlotServiceMock
                    .Setup(x => x.IsUniqueNumber(editVM.ParkingZoneId, editVM.Number))
                    .Returns(false);

            //Act
            var result = _controller.Edit(editVM, parkingSlot.Id);

            //Assert
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _parkingSlotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
            _parkingSlotServiceMock
                    .Verify(x => x.IsUniqueNumber(editVM.ParkingZoneId, editVM.Number), Times.Once);
        }

        [Fact]
        public void GivenEditVMAndParkingSlotId_WhenEditPostIsCalled_ThenModelStateIsValidReturnsToIndex()
        {
            //Arrange
            EditVM editVM = new(parkingSlot);
            editVM.Number = 123;
            var slot = editVM.MapToModel(parkingSlot);
            _parkingSlotServiceMock
                    .Setup(x => x.GetById(parkingSlot.Id))
                    .Returns(parkingSlot);

            _parkingSlotServiceMock.Setup(x => x.Update(slot));

            _parkingSlotServiceMock
                    .Setup(x => x.IsUniqueNumber(editVM.ParkingZoneId, editVM.Number))
                    .Returns(false);

            //Act
            var result = _controller.Edit(editVM, parkingSlot.Id);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(JsonSerializer.Serialize(result), JsonSerializer.Serialize(editVM));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(_controller.ModelState.IsValid);
            _parkingSlotServiceMock.Verify(x => x.Update(parkingSlot), Times.Once);
            _parkingSlotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
            _parkingSlotServiceMock
                    .Verify(x => x.IsUniqueNumber(editVM.ParkingZoneId, editVM.Number), Times.Once);
        }
        #endregion
    }
}
