using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ParkingZones;
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

        // GET: Admin/ParkingZones
        public IActionResult Index()
        {
            var parkingZones = _parkingZoneService.GetAll();
            var listItemVMs = ListItemVM.MapToVM(parkingZones);
            return View(listItemVMs);
        }

        // GET: Admin/ParkingZones/Details/5
        public IActionResult Details(Guid? id)
        {
            var parkingZone = _parkingZoneService.GetById(id);

            if (parkingZone is null)
                return NotFound();

            var detailsVM = new DetailsVM(parkingZone);
            return View(detailsVM);
        }

        // GET: Admin/ParkingZones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ParkingZones/Create
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

        // GET: Admin/ParkingZones/Edit/5
        public IActionResult Edit(Guid? id)
        {
            var parkingZone = _parkingZoneService.GetById(id);

            if (parkingZone is null)
                return NotFound();

            var parkingZoneEditVM = new EditVM(parkingZone);
            return View(parkingZoneEditVM);
        }

        // POST: Admin/ParkingZones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid? id, EditVM parkingZoneEditVM)
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingZoneExists(parkingZone.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(parkingZoneEditVM);
        }

        // GET: Admin/ParkingZones/Delete/5
        public IActionResult Delete(Guid? id)
        {
            var parkingZone = _parkingZoneService.GetById(id);

            if (parkingZone == null)
                return NotFound();

            return View(parkingZone);
        }

        // POST: Admin/ParkingZones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid? id)
        {
            var existingParkingZone = _parkingZoneService.GetById(id);
            if (existingParkingZone is null)
                return NotFound();

            _parkingZoneService.Remove(existingParkingZone);
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingZoneExists(Guid? id)
        {
            return _parkingZoneService.GetById(id) != null;
        }
    }
}
