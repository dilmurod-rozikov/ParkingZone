using Moq;
using ParkingZoneApp.Enums;
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
        private static readonly Guid slotId = Guid.NewGuid();
        private static readonly ParkingZone parkingZone = new()
        {
            Id = Guid.NewGuid(),
            Name = "Test Name",
            Address = "Test Address",
            CreatedDate = new(2024, 7, 7)
        };

        private static readonly ParkingSlot parkingSlot = new()
        {
            Id = slotId,
            Number = 1,
            Category = SlotCategory.Standard,
            IsAvailable = true,
            ParkingZoneId = parkingZone.Id,
            ParkingZone = parkingZone,
            Reservations = new List<Reservation>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    StartingTime = DateTime.UtcNow,
                    Duration = 1,
                    SlotId = slotId,
                    ZoneId = parkingZone.Id,
                    ParkingSlot = parkingSlot,
                }
            }
        };

        private readonly List<ParkingSlot> slots = [parkingSlot];

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
            _parkingSlotRepositoryMock
                    .Setup(x => x.GetAll())
                    .Returns(slots);

            //Act
            var result = _parkingSlotServiceMock.GetAll();

            //Assert
            Assert.IsType<List<ParkingSlot>>(result);
            Assert.Equal(JsonSerializer.Serialize(slots), JsonSerializer.Serialize(result));
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

        #region GetSlotByZoneId
        [Fact]
        public void GivenParkingZoneId_WhenGetSlotsByZoneIdIsCalled_ThenReturnCollectionOfSlots()
        {
            //Arrange
            _parkingSlotRepositoryMock.Setup(x => x.GetAll()).Returns(slots);

            //Act
            var result = _parkingSlotServiceMock.GetSlotsByZoneId(parkingZone.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<ParkingSlot>>(result);
            Assert.Equal(JsonSerializer.Serialize(result), JsonSerializer.Serialize(slots));
            _parkingSlotRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region IsUniqueNumber
        [Fact]
        public void GivenIdAndNumber_WhenIsUniqueNumberIsCalled_ThenReturnTrue()
        {
            //Arrange
            slots.Add(new() { Id = Guid.NewGuid(), Number = 2, ParkingZoneId = parkingZone.Id });
            _parkingSlotRepositoryMock.Setup(x => x.GetAll()).Returns(slots);

            //Act
            var result = _parkingSlotServiceMock.IsUniqueNumber(parkingSlot.ParkingZoneId, 3);

            //Assert
            Assert.False(result);
            _parkingSlotRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void GivenIdAndNumber_WhenIsUniqueNumberIsCalled_ThenReturnFalse()
        {
            //Arrange
            slots.Add(new() { Id = Guid.NewGuid(), Number = 1, ParkingZoneId = parkingZone.Id });
            _parkingSlotRepositoryMock.Setup(x => x.GetAll()).Returns(slots);

            //Act
            var result = _parkingSlotServiceMock.IsUniqueNumber(parkingSlot.ParkingZoneId, 1);

            //Assert
            Assert.True(result);
            _parkingSlotRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region IsSlotAvailableForReservation
        [Fact]
        public void GivenSlotStartTimeAndDuration_WhenIsSlotAvailableForReservationIsCalled_ThenReturnTrue()
        {
            //Arrange
           
            //Act
            var result = _parkingSlotServiceMock.IsSlotAvailableForReservation(parkingSlot, DateTime.UtcNow.AddHours(2), 2);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GivenSlotStartTimeAndDuration_WhenIsSlotAvailableForReservationIsCalled_ThenReturnFalse()
        {
            //Arrange

            //Act
            var result = _parkingSlotServiceMock.IsSlotAvailableForReservation(parkingSlot, DateTime.UtcNow, 2);

            //Assert
            Assert.False(result);
        }
        #endregion

        #region GetAllFreeSlots
        [Fact]
        public void GivenParkingZoneIdStartingTimeAndDuration_WhenGetAllFreeSlotsIsCalled_ThenReturnCollectionOfSlots()
        {
            //Arrange
            Reservation reservation = new()
            {
                Id = Guid.NewGuid(),
                StartingTime = DateTime.UtcNow,
                Duration = 1,
                SlotId = slotId,
                ZoneId = parkingZone.Id,
                ParkingSlot = parkingSlot,
            };
            _parkingSlotRepositoryMock.Setup(x => x.GetAll()).Returns(slots);

            //Act
            var result = _parkingSlotServiceMock
                .GetAllFreeSlots(parkingZone.Id, reservation.StartingTime, reservation.Duration);

            //Assert
            Assert.NotNull(result);
            _parkingSlotRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

    }
}
