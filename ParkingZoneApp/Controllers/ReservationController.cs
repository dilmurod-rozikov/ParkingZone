using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
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

        public IActionResult FreeSlots()
        {
            var zones = _parkingZoneService.GetAll().ToList();

            if (zones is null)
                return NotFound();

            FreeSlotsVMs freeSlotsVMs = new(zones);
            return View(freeSlotsVMs);
        }

        [HttpPost]
        public IActionResult FreeSlots(FreeSlotsVMs freeSlotsVMs)
        {
            var zones = _parkingZoneService.GetAll().ToList();

            if (zones is null)
                return NotFound();

            freeSlotsVMs.ListOfZones = new SelectList(zones, "Id", "Name");

            freeSlotsVMs.ParkingSlots = _parkingSlotService
                .GetAllFreeSlots(freeSlotsVMs.SelectedZoneId, freeSlotsVMs.StartingTime, freeSlotsVMs.Duration);

            return View(freeSlotsVMs);
        }

        [HttpGet]
        public IActionResult Reserve(Guid slotId, DateTime startTime, uint duration)
        {
            var slot = _parkingSlotService.GetById(slotId);

            if (slot is null)
                return NotFound();

            var zone = _parkingZoneService.GetById(slot.ParkingZoneId);
            ReserveVM reserveVM = new(duration, startTime, slot.Id, zone.Id, zone.Name, zone.Address, slot.Number);
            return View(reserveVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reserve(ReserveVM reserveVM)
        {
            var slot = _parkingSlotService.GetById(reserveVM.SlotId);

            if (slot is null)
                return NotFound();

            var zone = _parkingZoneService.GetById(reserveVM.ZoneId);

            if (zone is null)
                return NotFound();

            bool isSlotFree = _parkingSlotService
                .IsSlotFreeForReservation(slot, reserveVM.StartingTime, reserveVM.Duration);

            if (!isSlotFree)
            {
                ModelState.AddModelError("StartingTime", "Slot is not free for selected period");
            }
            else if (reserveVM.VehicleNumber.IsNullOrEmpty())
            {
                ModelState.AddModelError("VehicleNumber", "Vehicle number is required");
            }
            else
            {
                var reservation = reserveVM.MapToModel();
                reservation.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (reservation.UserId is null)
                    return NotFound();

                _reservationService.Insert(reservation);
                TempData["ReservationSuccess"] = "Parking slot reserved successfully.";
            }

            return View(reserveVM);
        }
    }
}
