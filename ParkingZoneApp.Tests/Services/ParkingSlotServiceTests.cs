﻿using Moq;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
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
            Reservations =
            [
                new()
                {
                    Id = Guid.NewGuid(),
                    StartingTime = DateTime.UtcNow,
                    Duration = 1,
                    ParkingSlotId = slotId,
                    ParkingZoneId = parkingZone.Id,
                    VehicleNumber = "Test-Number"
                }
            ]
        };

        private readonly List<ParkingSlot> slots = [parkingSlot];

        public ParkingSlotServiceTests()
        {
            _parkingSlotRepositoryMock = new Mock<IParkingSlotRepository>();
            _parkingSlotServiceMock = new ParkingSlotService(_parkingSlotRepositoryMock.Object);
        }

        #region Insert
        [Fact]
        public async Task GivenParkingSlotModel_WhenInsertIsCalled_ThenReturnNothing()
        {
            //Arrange
            _parkingSlotRepositoryMock.Setup(x => x.Add(parkingSlot));

            //Act
            await _parkingSlotServiceMock.Insert(parkingSlot);

            //Assert
            _parkingSlotRepositoryMock.Verify(x => x.Add(parkingSlot), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Update
        [Fact]
        public async Task GivenParkingSlotModel_WhenUpdateIsCalled_ThenReturnNothing()
        {
            //Arrange
            _parkingSlotRepositoryMock.Setup(x => x.Update(parkingSlot));

            //Act
            await _parkingSlotServiceMock.Update(parkingSlot);

            //Assert
            _parkingSlotRepositoryMock.Verify(x => x.Update(parkingSlot), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Remove
        [Fact]
        public async Task GivenParkingSlotModel_WhenRemoveIsCalled_ThenReturnNothing()
        {
            //Arrange
            _parkingSlotRepositoryMock.Setup(x => x.Delete(parkingSlot));

            //Act
            await _parkingSlotServiceMock.Remove(parkingSlot);

            //Assert
            _parkingSlotRepositoryMock.Verify(x => x.Delete(parkingSlot), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetAll
        [Fact]
        public async Task GivenParkingSlotModel_WhenGetAllIsCalled_ThenReturnAllSlots()
        {
            //Arrange
            _parkingSlotRepositoryMock
                    .Setup(x => x.GetAll())
                    .ReturnsAsync(slots);

            //Act
            var result = await _parkingSlotServiceMock.GetAll();

            //Assert
            Assert.IsType<List<ParkingSlot>>(result);
            Assert.Equal(JsonSerializer.Serialize(slots), JsonSerializer.Serialize(result));
            _parkingSlotRepositoryMock.Verify(x => x.GetAll());
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetById
        [Fact]
        public async Task GivenParkingSlotId_WhenGetByIdIsCalled_ThenReturnTheModel()
        {
            //Arrange
            _parkingSlotRepositoryMock
                    .Setup(x => x.GetByID(parkingSlot.Id))
                    .ReturnsAsync(parkingSlot);

            //Act
            var result = await _parkingSlotServiceMock.GetById(parkingSlot.Id);

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
        public async Task GivenParkingZoneId_WhenGetSlotsByZoneIdIsCalled_ThenReturnCollectionOfSlots()
        {
            //Arrange
            _parkingSlotRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(slots);

            //Act
            var result = await _parkingSlotServiceMock.GetSlotsByZoneIdAsync(parkingZone.Id);

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
        public async Task GivenIdAndNumber_WhenIsUniqueNumberIsCalled_ThenReturnTrue()
        {
            //Arrange
            slots.Add(new() { Id = Guid.NewGuid(), Number = 2, ParkingZoneId = parkingZone.Id });
            _parkingSlotRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(slots);

            //Act
            var result = await _parkingSlotServiceMock.IsUniqueNumberAsync(parkingSlot.ParkingZoneId, 3);

            //Assert
            Assert.False(result);
            _parkingSlotRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GivenIdAndNumber_WhenIsUniqueNumberIsCalled_ThenReturnFalse()
        {
            //Arrange
            slots.Add(new() { Id = Guid.NewGuid(), Number = 1, ParkingZoneId = parkingZone.Id });
            _parkingSlotRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(slots);

            //Act
            var result = await _parkingSlotServiceMock.IsUniqueNumberAsync(parkingSlot.ParkingZoneId, 1);

            //Assert
            Assert.True(result);
            _parkingSlotRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region IsSlotFreeForReservation
        public static IEnumerable<object[]> Data =>
        [
            // Slot is available for reservation
            [
                new List<Reservation>
                {
                    new() { StartingTime = new DateTime(2024, 5, 8, 9, 0, 0), Duration = 1u }
                },
                new DateTime(2024, 5, 8, 13, 0, 0),
                2,
                true
            ],
            // Slot is fully occupied
            [
                new List<Reservation>
                {
                    new() { StartingTime = new DateTime(2024, 5, 8, 9, 0, 0), Duration = 5u }
                },
                new DateTime(2024, 5, 8, 10, 0, 0),
                3,
                false
            ],
            // Slot is partially occupied
            [
                new List<Reservation>
                {
                    new() { StartingTime = new DateTime(2024, 5, 8, 12, 0, 0), Duration = 3u }
                },
                new DateTime(2024, 5, 8, 11, 0, 0),
                2,
                false
            ]
        ];

        [Theory]
        [MemberData(nameof(Data))]
        public void GivenSlotStartTimeAndDuration_WhenIsSlotFreeForReservationCalled_ThenReturnExpectedResult
            (List<Reservation> reservations, DateTime startTime, uint duration, bool expectedResult)
        {
            // Arrange
            ParkingSlot slot = new()
            {
                Reservations = reservations
            };

            // Act
            var result = _parkingSlotServiceMock.IsSlotFreeForReservation(slot, startTime, duration);

            // Assert
            Assert.Equal(expectedResult, result);
        }
        #endregion

        #region GetAllFreeSlots
        [Fact]
        public async Task GivenParkingZoneIdStartingTimeAndDuration_WhenGetAllFreeSlotsIsCalled_ThenReturnCollectionOfSlots()
        {
            //Arrange
            Reservation reservation = new()
            {
                Id = Guid.NewGuid(),
                StartingTime = DateTime.UtcNow,
                Duration = 1u,
                ParkingSlotId = slotId,
                ParkingZoneId = parkingZone.Id,
            };
            _parkingSlotRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(slots);

            //Act
            var result = await _parkingSlotServiceMock
                .GetAllFreeSlotsAsync(parkingZone.Id, reservation.StartingTime, reservation.Duration);

            //Assert
            Assert.NotNull(result);
            _parkingSlotRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            _parkingSlotRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Filter
        [Fact]
        public async Task GivenFilterVm_WhenFilterIsCalled_ThenReturnsQueriedCollection()
        {
            //Arrange
            FilterSlotVM filterVM = new()
            {
                ParkingZoneId = parkingZone.Id,
                Category = SlotCategory.Standard,
                IsSlotFree = true,
            };

            _parkingSlotRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(slots);

            //Act
            var result = await _parkingSlotServiceMock.FilterAsync(filterVM);

            //Assert
            var model = Assert.IsType<List<ParkingSlot>>(result);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<ParkingSlot>>(result);
            _parkingSlotRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }
        #endregion
    }
}
