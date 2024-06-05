using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.Security.Claims;

namespace ParkingZoneApp.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
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

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId.IsNullOrEmpty())
                return NotFound();

            var reservations = await _reservationService.GetReservationsByUserId(userId);
            var parkingZones = await _parkingZoneService.GetAll();
            var parkingSlots = await _parkingSlotService.GetAll();
            var indexVM = await ListItemVM.MapToVMAsync(reservations, parkingZones, parkingSlots);
            return View(indexVM);
        }

        [HttpGet]
        public async Task<IActionResult> Prolong(Guid reservationId)
        {
            var reservation = await _reservationService.GetById(reservationId);

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
        public async Task<IActionResult> Prolong(ProlongVM prolongVM)
        {
            var reservation = await _reservationService.GetById(prolongVM.Id);

            if (reservation is null)
                return NotFound();

            prolongVM.StartTime = reservation.StartingTime;
            prolongVM.FinishTime = reservation.StartingTime.AddHours(reservation.Duration);
            var slot = await _parkingSlotService.GetById(reservation.ParkingSlotId);
            bool isProlongable = _parkingSlotService
                .IsSlotFreeForReservation(slot, prolongVM.StartTime.AddHours(reservation.Duration), prolongVM.ProlongDuration);

            if (!isProlongable)
            {
                ModelState.AddModelError("ProlongDuration", "This slot is already reserved for chosen prolong time!");
            }

            if (ModelState.IsValid)
            {
                reservation = prolongVM.MapToModel(reservation);
                await _reservationService.Update(reservation);
                TempData["SuccessMessage"] = "Reservation successfully prolonged.";
            }

            return View(prolongVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid reservationId)
        {
            var reservation = await _reservationService.GetById(reservationId);
            if (reservation is null)
                return NotFound();

            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var reservation = await _reservationService.GetById(id);
            if (reservation is null)
                return NotFound();

            await _reservationService.Remove(reservation);
            return RedirectToAction(nameof(Index));
        }
    }
}