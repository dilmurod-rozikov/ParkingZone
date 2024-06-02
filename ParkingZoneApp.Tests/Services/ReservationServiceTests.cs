using Moq;
using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Repository.Interfaces;
using ParkingZoneApp.Services;
using ParkingZoneApp.Services.Interfaces;
using System.Text.Json;

namespace ParkingZoneApp.Tests.Services
{
    public class ReservationServiceTests
    {
        private readonly Mock<IReservationRepository> reservationRepositoryMock;
        private readonly IReservationService reservationService;
        private static readonly ParkingZone parkingZone = new() { Id = Guid.NewGuid(), }; 
        private static readonly Reservation reservation = new()
        {
            Id = Guid.NewGuid(),
            Duration = 5,
            ParkingSlotId = Guid.NewGuid(),
            StartingTime = DateTime.Now.AddHours(-1),
            ParkingZoneId = parkingZone.Id,
            UserId = "Test-user-id",
        };

        private readonly IEnumerable<Reservation> reservations = [reservation];

        public ReservationServiceTests()
        {
            reservationRepositoryMock = new Mock<IReservationRepository>();
            reservationService = new ReservationService(reservationRepositoryMock.Object);
        }

        #region Insert
        [Fact]
        public async Task GivenReservationModel_WhenInsertIsCalled_ThenReturnNothing()
        {
            //Arrange
            reservationRepositoryMock.Setup(x => x.Add(reservation));

            //Act
            await reservationService.Insert(reservation);

            //Assert
            reservationRepositoryMock.Verify(x => x.Add(reservation), Times.Once);
            reservationRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Update
        [Fact]
        public async Task GivenReservationModel_WhenUpdateIsCalled_ThenReturnNothing()
        {
            //Arrange
            reservationRepositoryMock.Setup(x => x.Update(reservation));

            //Act
            await reservationService.Update(reservation);

            //Assert
            reservationRepositoryMock.Verify(x => x.Update(reservation), Times.Once);
            reservationRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Delete
        [Fact]
        public async Task GivenParkingZoneModel_WhenRemoveIsCalled_ThenReturnNothing()
        {
            //Arrange
            reservationRepositoryMock.Setup(x => x.Delete(reservation));

            //Act
            await reservationService.Remove(reservation);

            //Assert
            reservationRepositoryMock.Verify(x => x.Delete(reservation), Times.Once);
            reservationRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetAll
        [Fact]
        public async Task GivenNothing_WhenGetAllIsCalled_ThenReturnAllModels()
        {
            //Arrange
            IEnumerable<Reservation> expectedReservations = [reservation];
            reservationRepositoryMock
                    .Setup(x => x.GetAll())
                    .ReturnsAsync(expectedReservations);
            //Act
            var result = await reservationService.GetAll();

            //Assert
            Assert.Equal(JsonSerializer.Serialize(expectedReservations), JsonSerializer.Serialize(result));
            reservationRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            reservationRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetByID
        [Fact]
        public async Task GivenGuid_WhenGetByIdIsCalled_ThenReturnModel()
        {
            //Arrange
            reservationRepositoryMock
                    .Setup(x => x.GetByID(reservation.Id))
                    .ReturnsAsync(reservation);

            //Act
            var result = await reservationService.GetById(reservation.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Reservation>(result);
            Assert.Equal(JsonSerializer.Serialize(reservation), JsonSerializer.Serialize(result));
            reservationRepositoryMock.Verify(x => x.GetByID(reservation.Id), Times.Once);
            reservationRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetReservationsByUser
        [Fact]
        public async Task GivenUserId_WhenGetReservationsByUserIsCalled_ThenReturnsListOfReservations()
        {
            //Arrage
            reservationRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(reservations);

            //Act
            var result = await reservationService.GetReservationsByUserId(reservation.UserId);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Reservation>>(result);
            Assert.Equal(JsonSerializer.Serialize(reservations), JsonSerializer.Serialize(result));
            reservationRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            reservationRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetReservationsByZoneId
        [Fact]
        public async Task GivenZoneId_WhenGetReservationsByZoneIdIsCalled_ThenReturnListOfReservations()
        {
            //Arrange
            reservationRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(reservations);

            //Act
            var result = await reservationService.GetReservationsByZoneId(parkingZone.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Reservation>>(result);
            Assert.Equal(JsonSerializer.Serialize(result), JsonSerializer.Serialize(reservations));
            reservationRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }
        #endregion
    }
}
