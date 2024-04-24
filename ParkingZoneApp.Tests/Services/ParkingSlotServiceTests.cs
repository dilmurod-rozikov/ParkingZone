using Moq;
using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services;
using ParkingZoneApp.Services.Interfaces;
using System.Text.Json;

namespace ParkingZoneApp.Tests.Services
{
    public class ParkingSlotServiceTests
    {
        private readonly Mock<IParkingSlotRepository> _parkingSlotRepositoryMock;
        private readonly IParkingSlotService _parkingSlotServiceMock;
        private static readonly ParkingZone parkingZone = new ParkingZone()
        {
            Id = Guid.NewGuid(),
            Name = "Test Name",
            Address = "Test Address",
            CreatedDate = new DateOnly(2024, 7, 7)
        };

        private readonly ParkingSlot parkingSlot = new()
        {
            Id = Guid.NewGuid(),
            Number = 1, 
            Category = Models.Enums.SlotCategory.Standard,
            IsAvailable = false,
            ParkingZoneId = parkingZone.Id,
            ParkingZone = parkingZone
        };

        public ParkingSlotServiceTests()
        {
            _parkingSlotRepositoryMock = new Mock<IParkingSlotRepository>();
            _parkingSlotServiceMock = new ParkingSlotService(_parkingSlotRepositoryMock.Object);
        }

        #region Insert
        [Fact]
        public void GivenParkingSlotModel_WhenInsertIsCalled_ThenReturnNothing()
        {
            //Arrange
            _parkingSlotRepositoryMock.Setup(x => x.Add(parkingSlot));

            //Act
            _parkingSlotServiceMock.Insert(parkingSlot);

            //Assert
            _parkingSlotRepositoryMock.Verify(x => x.Add(parkingSlot), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Update
        [Fact]
        public void GivenParkingSlotModel_WhenUpdateIsCalled_ThenReturnNothing()
        {
            //Arrange
            _parkingSlotRepositoryMock.Setup(x => x.Update(parkingSlot));

            //Act
            _parkingSlotServiceMock.Update(parkingSlot);

            //Assert
            _parkingSlotRepositoryMock.Verify(x => x.Update(parkingSlot), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Remove
        [Fact]
        public void GivenParkingSlotModel_WhenRemoveIsCalled_ThenReturnNothing()
        {
            //Arrange
            _parkingSlotRepositoryMock.Setup(x => x.Delete(parkingSlot));

            //Act
            _parkingSlotServiceMock.Remove(parkingSlot);

            //Assert
            _parkingSlotRepositoryMock.Verify(x => x.Delete(parkingSlot), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetAll
        [Fact]
        public void GivenParkingSlotModel_WhenGetAllIsCalled_ThenReturnAllSlots()
        {
            //Arrange
            var expected = new List<ParkingSlot>()
            {
                parkingSlot
            };
            _parkingSlotRepositoryMock
                    .Setup(x => x.GetAll())
                    .Returns(expected);

            //Act
            var result = _parkingSlotServiceMock.GetAll();

            //Assert
            Assert.IsType<List<ParkingSlot>>(result);
            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(result));
            _parkingSlotRepositoryMock.Verify(x => x.GetAll());
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetById
        [Fact]
        public void GivenParkingSlotId_WhenGetByIdIsCalled_ThenReturnTheModel()
        {
            //Arrange
            _parkingSlotRepositoryMock
                    .Setup(x => x.GetByID(parkingSlot.Id))
                    .Returns(parkingSlot);

            //Act
            var result = _parkingSlotServiceMock.GetById(parkingSlot.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ParkingSlot>(result);
            Assert.Equal(JsonSerializer.Serialize(parkingSlot), JsonSerializer.Serialize(result));
            _parkingSlotRepositoryMock.Verify(x => x.GetByID(parkingSlot.Id), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }

        #endregion

    }
}
