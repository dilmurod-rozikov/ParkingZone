using Microsoft.AspNetCore.Mvc;
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
            ParkingSlots = [parkingSlot]
        };

        private static readonly ParkingSlot parkingSlot = new()
        {
            Id = Guid.NewGuid(),
            Number = 1,
            Category = SlotCategory.Standard,
            IsAvailable = true,
            ParkingZoneId = parkingZone.Id,
            ParkingZone = parkingZone,
            Reservations =
            [
                new()
                {
                    Id = Guid.NewGuid(),
                    StartingTime = DateTime.Now.AddHours(-2),
                    Duration = 1
                }
            ]
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

        [Fact]
        public void GivenFilterSlotVMWithCategoryAndIsSlotFree_WhenPostIndexIsCalled_ThenReturnsPartialView()
        {
            //Arrange
            var slots = new List<ParkingSlot>
            {
                parkingSlot
            };
            var filterSlotVM = new FilterSlotVM
            {
                ParkingZoneId = parkingZone.Id,
                Category = SlotCategory.Standard,
                IsSlotFree = true
            };
            _parkingSlotServiceMock.Setup(x => x.GetSlotsByZoneId(parkingZone.Id)).Returns(slots);
            _parkingSlotServiceMock.Setup(x => x.FilterByCategory(slots, filterSlotVM.Category)).Returns(slots);
            _parkingSlotServiceMock.Setup(x => x.FilterByFreeSlot(slots, filterSlotVM.IsSlotFree)).Returns(slots);

            //Act
            var result = _controller.Index(filterSlotVM);

            //Assert
            Assert.IsType<PartialViewResult>(result);
            Assert.NotNull(result);
            Assert.Equal("_FilteredSlotsPartial", ((PartialViewResult)result).ViewName);
            _parkingSlotServiceMock.Verify(x => x.GetSlotsByZoneId(parkingZone.Id), Times.Once);
            _parkingSlotServiceMock.Verify(x => x.FilterByCategory(slots, filterSlotVM.Category), Times.Once);
            _parkingSlotServiceMock.Verify(x => x.FilterByFreeSlot(slots, filterSlotVM.IsSlotFree), Times.Once);
        }

        [Fact]
        public void GivenFilterSlotVMWithCategory_WhenPostIndexIsCalled_ThenReturnsPartialView()
        {
            //Arrange
            var slots = new List<ParkingSlot>
            {
                parkingSlot
            };
            _parkingSlotServiceMock.Setup(x => x.GetSlotsByZoneId(parkingZone.Id)).Returns(slots);
            var filterSlotVM = new FilterSlotVM
            {
                ParkingZoneId = parkingZone.Id,
                Category = SlotCategory.Standard,
                IsSlotFree = null
            };
            _parkingSlotServiceMock.Setup(x => x.FilterByCategory(slots, filterSlotVM.Category)).Returns(slots);

            //Act
            var result = _controller.Index(filterSlotVM) as PartialViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("_FilteredSlotsPartial", result.ViewName);
            _parkingSlotServiceMock.Verify(x => x.GetSlotsByZoneId(parkingZone.Id), Times.Once);
            _parkingSlotServiceMock.Verify(x => x.FilterByCategory(slots, filterSlotVM.Category), Times.Once);
        }

        [Fact]
        public void GivenFilterSlotVMWithIsSlotFree_WhenPostIndexIsCalled_ThenReturnsPartialView()
        {
            //Arrange
            var slots = new List<ParkingSlot>
            {
                parkingSlot
            };
            _parkingSlotServiceMock.Setup(x => x.GetSlotsByZoneId(parkingZone.Id)).Returns(slots);
            var filterSlotVM = new FilterSlotVM
            {
                ParkingZoneId = parkingZone.Id,
                Category = null,
                IsSlotFree = true
            };
            _parkingSlotServiceMock.Setup(x => x.FilterByFreeSlot(slots, filterSlotVM.IsSlotFree)).Returns(slots);

            //Act
            var result = _controller.Index(filterSlotVM) as PartialViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("_FilteredSlotsPartial", result.ViewName);
            _parkingSlotServiceMock.Verify(x => x.GetSlotsByZoneId(parkingZone.Id), Times.Once);
            _parkingSlotServiceMock.Verify(x => x.FilterByFreeSlot(slots, filterSlotVM.IsSlotFree), Times.Once);
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
        public void GivenEditVMAndParkingSlotId_WhenEditPostIsCalled_ThenReturnNotFoundIfIdAndSlotIdDoesNotMatch()
        {
            //Arrange
            EditVM editVM = new(parkingSlot);

            //Act
            var result = _controller.Edit(editVM, parkingSlot.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GivenEditVMAndParkingSlotId_WhenEditPostIsCalled_ThenReturnNotFoundIfSlotIsNull()
        {
            //Arrange
            EditVM editVM = new(parkingSlot)
            {
                Id = Guid.Empty
            };
            _parkingSlotServiceMock.Setup(x => x.GetById(editVM.Id));

            //Act
            var result = _controller.Edit(editVM, editVM.Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            _parkingSlotServiceMock.Verify(x => x.GetById(editVM.Id), Times.Once);
        }

        [Fact]
        public void GivenEditVMAndParkingSlotId_WhenEditPostIsCalled_ThenNumberIsNotUniqueAndModelStateIsFalseAndReturnsViewResult()
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
        public void GivenEditVMAndParkingSlotId_WhenEditPostIsCalled_ThenSlotIsInUseAndModelStateIsFalseAndReturnsViewResult()
        {
            //Arrange
            EditVM editVM = new(parkingSlot);
            _controller.ModelState.AddModelError("Category", "This slot is in use, category cannot be modified!");
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
            EditVM editVM = new(parkingSlot)
            {
                Number = 123,
                IsSlotInUse = false,
            };
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

        #region Details
        [Fact]
        public void GivenParkingSlotId_WhenGetDetailsIsCalled_ThenReturnViewResult()
        {
            //Arrange
            DetailsVM expectedVM = new(parkingSlot);
            _parkingSlotServiceMock.Setup(x => x.GetById(parkingSlot.Id)).Returns(parkingSlot);

            //Act
            var result = _controller.Details(parkingSlot.Id);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(model), JsonSerializer.Serialize(expectedVM));
            _parkingSlotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
        }

        [Fact]
        public void GivenParkingSlotId_WhenGetDetailsIsCalled_ThenReturnNotFound()
        {
            //Arrange
            _parkingSlotServiceMock.Setup(x => x.GetById(parkingSlot.Id));

            //Act
            var result = _controller.Details(parkingSlot.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _parkingSlotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
        }
        #endregion

        #region Delete
        [Fact]
        public void GivenParkingSlotId_WhenGetDeleteIsCalled_ThenReturnsNotFoundResult()
        {
            //Arrange
            _parkingSlotServiceMock.Setup(x => x.GetById(parkingSlot.Id));

            //Act
            var result = _controller.Delete(parkingSlot.Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            _parkingSlotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
        }

        [Fact]
        public void GivenParkingSlotId_WhenGetDeleteIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            _parkingSlotServiceMock
                    .Setup(x => x.GetById(parkingSlot.Id))
                    .Returns(parkingSlot);

            //Act
            var result = _controller.Delete(parkingSlot.Id);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.Equal(JsonSerializer.Serialize(model), JsonSerializer.Serialize(parkingSlot));
            _parkingSlotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
        }

        [Fact]
        public void GivenParkingSlotId_WhenPostDeleteIsCalled_ThenReturnsNotFoundResult()
        {
            //Arrange
            _parkingSlotServiceMock.Setup(x => x.GetById(parkingSlot.Id));

            //Act
            var result = _controller.Delete(parkingSlot.Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            _parkingSlotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
        }

        [Fact]
        public void GivenParkingSlotId_WhenPostDeleteIsCalled_ThenIfSlotIsInUseReturnsModelError()
        {
            //Arrange
            _controller.ModelState.AddModelError("DeleteButton", "This slot is in use, cannot be deleted!");
            _parkingSlotServiceMock.Setup(x => x.GetById(parkingSlot.Id)).Returns(parkingSlot);

            //Act
            var result = _controller.Delete(parkingSlot.Id);

            //Assert
            Assert.IsType<ViewResult>(result);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _parkingSlotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
        }

        [Fact]
        public void GivenParkingSlotId_WhenPostDeleteIsCalled_ThenReturnsRedirectToActionResult()
        {
            //Arrange
            _parkingSlotServiceMock.Setup(x => x.GetById(parkingSlot.Id)).Returns(parkingSlot);
            _parkingSlotServiceMock.Setup(x => x.Remove(parkingSlot));

            //Act
            var result = _controller.DeleteConfirmed(parkingSlot.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
            _parkingSlotServiceMock.Verify(x => x.GetById(parkingSlot.Id), Times.Once);
            _parkingSlotServiceMock.Verify(x => x.Remove(parkingSlot), Times.Once);
        }
        #endregion
    }
}