using Moq;
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
        private readonly Reservation reservation = new()
        {
            Id = Guid.NewGuid(),
            Duration = 2,
            ParkingSlotId = Guid.NewGuid(),
            StartingTime = DateTime.UtcNow,
            ParkingZoneId = Guid.NewGuid(),
        };

        public ReservationServiceTests()
        {
            reservationRepositoryMock = new Mock<IReservationRepository>();
            reservationService = new ReservationService(reservationRepositoryMock.Object);
        }

        #region Insert
        [Fact]
        public void GivenReservationModel_WhenInsertIsCalled_ThenReturnNothing()
        {
            //Arrange
            reservationRepositoryMock.Setup(x => x.Add(reservation));

            //Act
            reservationService.Insert(reservation);

            //Assert
            reservationRepositoryMock.Verify(x => x.Add(reservation), Times.Once);
            reservationRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Update
        [Fact]
        public void GivenReservationModel_WhenUpdateIsCalled_ThenReturnNothing()
        {
            //Arrange
            reservationRepositoryMock.Setup(x => x.Update(reservation));

            //Act
            reservationService.Update(reservation);

            //Assert
            reservationRepositoryMock.Verify(x => x.Update(reservation), Times.Once);
            reservationRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Delete
        [Fact]
        public void GivenParkingZoneModel_WhenRemoveIsCalled_ThenReturnNothing()
        {
            //Arrange
            reservationRepositoryMock.Setup(x => x.Delete(reservation));

            //Act
            reservationService.Remove(reservation);

            //Assert
            reservationRepositoryMock.Verify(x => x.Delete(reservation), Times.Once);
            reservationRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetAll
        [Fact]
        public void GivenNothing_WhenGetAllIsCalled_ThenReturnAllModels()
        {
            //Arrange
            IEnumerable<Reservation> expectedReservations = new List<Reservation>()
            {
                reservation
            };
            reservationRepositoryMock
                    .Setup(x => x.GetAll())
                    .Returns(expectedReservations);
            //Act
            var result = reservationService.GetAll();

            //Assert
            Assert.Equal(JsonSerializer.Serialize(expectedReservations), JsonSerializer.Serialize(result));
            reservationRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            reservationRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetByID
        [Fact]
        public void GivenGuid_WhenGetByIdIsCalled_ThenReturnModel()
        {
            //Arrange
            reservationRepositoryMock
                    .Setup(x => x.GetByID(reservation.Id))
                    .Returns(reservation);

            //Act
            var result = reservationService.GetById(reservation.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Reservation>(result);
            Assert.Equal(JsonSerializer.Serialize(reservation), JsonSerializer.Serialize(result));
            reservationRepositoryMock.Verify(x => x.GetByID(reservation.Id), Times.Once);
            reservationRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion
    }
}
