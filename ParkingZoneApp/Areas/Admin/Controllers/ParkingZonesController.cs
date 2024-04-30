using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ParkingZoneVMs;
namespace ParkingZoneApp.Areas.Admin
{
    [Area("Admin")]
    [Authorize]
    public class ParkingZonesController : Controller
    {
        private readonly IParkingZoneService _parkingZoneService;
        public ParkingZonesController(IParkingZoneService parkingZoneService)
        {
            _parkingZoneService = parkingZoneService;
        }

        public IActionResult Index()
        {
            var parkingZones = _parkingZoneService.GetAll();
            var listItemVMs = ListItemVM.MapToVM(parkingZones);
            return View(listItemVMs);
        }

        public IActionResult Details(Guid id)
        {
            var parkingZone = _parkingZoneService.GetById(id);

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
        public IActionResult Create(CreateVM parkingZoneCreateVM)
        {
            if (ModelState.IsValid)
            {
                var parkingZone = parkingZoneCreateVM.MapToModel();
                _parkingZoneService.Insert(parkingZone);
                return RedirectToAction(nameof(Index));
            }

            return View(parkingZoneCreateVM);
        }

        public IActionResult Edit(Guid id)
        {
            var parkingZone = _parkingZoneService.GetById(id);

            if (parkingZone is null)
                return NotFound();

            var parkingZoneEditVM = new EditVM(parkingZone);
            return View(parkingZoneEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, EditVM parkingZoneEditVM)
        {
            var parkingZone = _parkingZoneService.GetById(id);

            if (parkingZone is null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    parkingZone = parkingZoneEditVM.MapToModel(parkingZone);
                    _parkingZoneService.Update(parkingZone);
                }
                catch (DbUpdateConcurrencyException) when (!ParkingZoneExists(parkingZone.Id))
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            return View(parkingZoneEditVM);
        }

        public IActionResult Delete(Guid id)
        {
            var parkingZone = _parkingZoneService.GetById(id);

            if (parkingZone == null)
                return NotFound();

            return View(parkingZone);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var existingParkingZone = _parkingZoneService.GetById(id);
            if (existingParkingZone is null)
                return NotFound();

            _parkingZoneService.Remove(existingParkingZone);
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingZoneExists(Guid id)
        {
            return _parkingZoneService.GetById(id) != null;
        }
    }
}
