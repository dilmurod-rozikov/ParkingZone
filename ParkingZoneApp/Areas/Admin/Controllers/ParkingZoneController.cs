using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ParkingZoneVMs;
namespace ParkingZoneApp.Areas.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ParkingZoneController : Controller
    {
        private readonly IParkingZoneService _parkingZoneService;
        private readonly IReservationService _reservationService;
        public ParkingZoneController(IParkingZoneService parkingZoneService,IReservationService reservationService)
        {
            _parkingZoneService = parkingZoneService;
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            var parkingZones = await _parkingZoneService.GetAll();
            var listItemVMs = ListItemVM.MapToVM(parkingZones);
            return View(listItemVMs);
        }

        public async Task<IActionResult> GetCurrentCars(Guid zoneId)
        {
            var zone = await _parkingZoneService.GetById(zoneId);
            if (zone is null)
                return NotFound();

            ViewData["ZoneName"] = zone.Name;
            var reservations = await _reservationService.GetReservationsByZoneId(zoneId);
            var vms = reservations.Select(x => new GetCurrentCarsVM(x));
            return View(vms);
        }

        public async Task<IActionResult> FilterByPeriod(Guid zoneId, PeriodRange periodRange)
        {
            var zone = await _parkingZoneService.GetById(zoneId);
            if(zone is null)
                return NotFound();

            var result = _parkingZoneService.FilterByPeriodOnSlotCategory(zone, periodRange);
            var myObj = new { categoryHours = result };
            return Json(myObj);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var parkingZone = await _parkingZoneService.GetById(id);

            if (parkingZone is null)
                return NotFound();

            var detailsVM = new DetailsVM(parkingZone);
            return View(detailsVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVM parkingZoneCreateVM)
        {
            if (ModelState.IsValid)
            {
                var parkingZone = parkingZoneCreateVM.MapToModel();
                await _parkingZoneService.Insert(parkingZone);
                return RedirectToAction(nameof(Index));
            }

            return View(parkingZoneCreateVM);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var parkingZone = await _parkingZoneService.GetById(id);

            if (parkingZone is null)
                return NotFound();

            var parkingZoneEditVM = new EditVM(parkingZone);
            return View(parkingZoneEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditVM parkingZoneEditVM)
        {
            var parkingZone = await _parkingZoneService.GetById(id);

            if (parkingZone is null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    parkingZone = parkingZoneEditVM.MapToModel(parkingZone);
                    await _parkingZoneService.Update(parkingZone);
                }
                catch (DbUpdateConcurrencyException) when (!ParkingZoneExists(parkingZone.Id))
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            return View(parkingZoneEditVM);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var parkingZone = await _parkingZoneService.GetById(id);

            if (parkingZone == null)
                return NotFound();

            return View(parkingZone);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var existingParkingZone = await _parkingZoneService.GetById(id);
            if (existingParkingZone is null)
                return NotFound();

            await _parkingZoneService.Remove(existingParkingZone);
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingZoneExists(Guid id)
        {
            return _parkingZoneService.GetById(id) != null;
        }
    }
}
