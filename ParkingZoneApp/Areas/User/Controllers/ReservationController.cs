using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.Security.Claims;

namespace ParkingZoneApp.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IParkingZoneService _parkingZoneService;
        private readonly IParkingSlotService _parkingSlotService;
        public ReservationController(
            IReservationService reservationService,
            IParkingZoneService parkingZoneService,
            IParkingSlotService parkingSlotService)
        {
            _reservationService = reservationService;
            _parkingZoneService = parkingZoneService;
            _parkingSlotService = parkingSlotService;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId.IsNullOrEmpty())
                return NotFound();

            var reservations = _reservationService.GetReservationsByUserId(userId);
            var indexVM = ListItemVM.MapToVM(reservations, _parkingZoneService, _parkingSlotService);
            return View(indexVM);
        }

        [HttpGet]
        public IActionResult Prolong(Guid reservationId)
        {
            var reservation = _reservationService.GetById(reservationId);

            if (reservation is null)
                return NotFound();

            ProlongVM prolongVM = new()
            {
                Id = reservationId,
                StartTime = reservation.StartingTime,
                FinishTime = reservation.StartingTime.AddHours(reservation.Duration),
            };

            return View(prolongVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Prolong(ProlongVM prolongVM)
        {
            var reservation = _reservationService.GetById(prolongVM.Id);

            if (reservation is null)
                return NotFound();

            if (ModelState.IsValid)
            {
                reservation = prolongVM.MapToModel(reservation);
                _reservationService.Update(reservation);
                TempData["SuccessMessage"] = "Reservation successfully prolonged.";
            }

            prolongVM.StartTime = reservation.StartingTime;
            prolongVM.FinishTime = reservation.StartingTime.AddHours(reservation.Duration);
            return View(prolongVM);
        }
    }
}
