using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.Security.Claims;

namespace ParkingZoneApp.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> FreeSlots()
        {
            var zones = await _parkingZoneService.GetAll();

            if (zones is null)
                return NotFound();

            FreeSlotsVM freeSlotsVMs = new(zones);
            return View(freeSlotsVMs);
        }

        [HttpPost]
        public async Task<IActionResult> FreeSlots(FreeSlotsVM freeSlotsVMs)
        {
            var zones = await _parkingZoneService.GetAll();
            if (zones is null)
                return NotFound();

            freeSlotsVMs.ListOfZones = new SelectList(zones, "Id", "Name");
            freeSlotsVMs.ParkingSlots = await _parkingSlotService
                .GetAllFreeSlotsAsync(freeSlotsVMs.SelectedZoneId, freeSlotsVMs.StartingTime, freeSlotsVMs.Duration);

            return View(freeSlotsVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Reserve(Guid slotId, DateTime startTime, uint duration)
        {
            var slot = await _parkingSlotService.GetById(slotId);
            if (slot is null)
                return NotFound();

            var zone = await _parkingZoneService.GetById(slot.ParkingZoneId);
            ReserveVM reserveVM = new(duration, startTime, slot.Id, zone.Id, zone.Name, zone.Address, slot.Number);
            return View(reserveVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(ReserveVM reserveVM)
        {
            var slot = await _parkingSlotService.GetById(reserveVM.SlotId);
            if (slot is null)
                return NotFound();

            var zone = await _parkingZoneService.GetById(reserveVM.ZoneId);
            if (zone is null)
                return NotFound();

            bool isSlotFree = _parkingSlotService
                .IsSlotFreeForReservation(slot, reserveVM.StartingTime, reserveVM.Duration);
            if (!isSlotFree)
                ModelState.AddModelError("StartingTime", "Slot is not free for selected period");

            if (ModelState.IsValid)
            {
                var reservation = reserveVM.MapToModel();
                reservation.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (reservation.UserId == null)
                    return NotFound();

                await _reservationService.Insert(reservation);
                TempData["ReservationSuccess"] = "Parking slot reserved successfully.";
            }

            return View(reserveVM);
        }
    }
}
