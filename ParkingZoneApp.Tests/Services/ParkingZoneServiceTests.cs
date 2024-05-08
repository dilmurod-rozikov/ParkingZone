using Moq;
using ParkingZoneApp.Models;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services;
using ParkingZoneApp.Services.Interfaces;
using System.Text.Json;

namespace ParkingZoneApp.Tests.Services
{
    public class ParkingZoneServiceTests
    {
        private readonly Mock<IParkingZoneRepository> _parkingZoneRepositoryMock;
        private readonly IParkingZoneService _parkingZoneServiceMock;
        private readonly ParkingZone parkingZone = new ParkingZone()
        {
            Id = Guid.NewGuid(),
            Name = "Test Name",
            Address = "Test Address",
            CreatedDate = new DateOnly(2024, 7, 7)
        };

        public ParkingZoneServiceTests()
        {
            _parkingZoneRepositoryMock = new Mock<IParkingZoneRepository>();
            _parkingZoneServiceMock = new ParkingZoneService(_parkingZoneRepositoryMock.Object);
        }

        #region Insert
        [Fact]
        public void GivenParkingZoneModel_WhenInsertIsCalled_ThenReturnNothing()
        {
            //Arrange
            _parkingZoneRepositoryMock.Setup(x => x.Add(parkingZone));

            //Act
            _parkingZoneServiceMock.Insert(parkingZone);

            //Assert
            _parkingZoneRepositoryMock.Verify(x => x.Add(parkingZone), Times.Once);
            _parkingZoneRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Update
        [Fact]
        public void GivenParkingZoneModel_WhenUpdateIsCalled_ThenReturnNothing()
        {
            //Arrange
            _parkingZoneRepositoryMock.Setup(x => x.Update(parkingZone));

            //Act
            _parkingZoneServiceMock.Update(parkingZone);

            //Assert
            _parkingZoneRepositoryMock.Verify(x => x.Update(parkingZone), Times.Once);
            _parkingZoneRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Delete
        [Fact]
        public void GivenParkingZoneModel_WhenRemoveIsCalled_ThenReturnNothing()
        {
            //Arrange
            _parkingZoneRepositoryMock.Setup(x => x.Delete(parkingZone));

            //Act
            _parkingZoneServiceMock.Remove(parkingZone);

            //Assert
            _parkingZoneRepositoryMock.Verify(x => x.Delete(parkingZone), Times.Once);
            _parkingZoneRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetAll
        [Fact]
        public void GivenNothing_WhenGetAllIsCalled_ThenReturnAllModels()
        {
            //Arrange
            IEnumerable<ParkingZone> expectedZones = new List<ParkingZone>()
            {
                parkingZone
            };
            _parkingZoneRepositoryMock
                    .Setup(x => x.GetAll())
                    .Returns(expectedZones);
            //Act
            var result = _parkingZoneServiceMock.GetAll();

            //Assert
            Assert.Equal(JsonSerializer.Serialize(expectedZones), JsonSerializer.Serialize(result));
            _parkingZoneRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            _parkingZoneRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetByID
        [Fact]
        public void GivenGuid_WhenGetByIdIsCalled_ThenReturnModel()
        {
            //Arrange
            _parkingZoneRepositoryMock
                    .Setup(x => x.GetByID(parkingZone.Id))
                    .Returns(parkingZone);

            //Act
            var result = _parkingZoneServiceMock.GetById(parkingZone.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ParkingZone>(result);
            Assert.Equal(JsonSerializer.Serialize(parkingZone), JsonSerializer.Serialize(result));
            _parkingZoneRepositoryMock.Verify(x => x.GetByID(parkingZone.Id), Times.Once);
            _parkingZoneRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion
    }
}
