using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services.Interfaces;
namespace ParkingZoneApp.Areas.Admin
{
    [Area("Admin")]
    [Authorize]
    public class ParkingZonesController : Controller
    {

        private readonly IParkingZoneServices _parkingZoneService;
        public ParkingZonesController(IParkingZoneServices parkingZoneService)
        {
            _parkingZoneService = parkingZoneService;
        }

        // GET: Admin/ParkingZones
        public ActionResult Index()
        {
            var entity = _parkingZoneService.RetrieveAll();
            return View(entity);
        }

        // GET: Admin/ParkingZones/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var parkingZone = _parkingZoneService.GetById(id);

            if (parkingZone == null)
                return NotFound();

            return View(parkingZone);
        }

        // GET: Admin/ParkingZones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ParkingZones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ParkingZone parkingZone)
        {
            if (ModelState.IsValid)
            {
                parkingZone.Id = Guid.NewGuid();
                _parkingZoneService.Insert(parkingZone);
                return RedirectToAction(nameof(Index));
            }

            return View(parkingZone);
        }

        // GET: Admin/ParkingZones/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var parkingZone = _parkingZoneService.GetById(id);

            if (parkingZone == null)
                return NotFound();

            _parkingZoneService.Update(parkingZone);
            return View(parkingZone);
        }

        // POST: Admin/ParkingZones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, ParkingZone parkingZone)
        {
            if (id != parkingZone.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
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

            return View(parkingZone);
        }

        // GET: Admin/ParkingZones/Delete/5
        public ActionResult Delete(Guid id)
        {
            var parkingZone = _parkingZoneService.GetById(id);
            if (parkingZone == null)
                return NotFound();
            else
                _parkingZoneService.Remove(parkingZone);

            return View(parkingZone);
        }

        // POST: Admin/ParkingZones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var parkingZone = _parkingZoneService.GetById(id);
            if (parkingZone != null)
            {
                _parkingZoneService.Remove(parkingZone);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ParkingZoneExists(Guid id)
        {
            return _parkingZoneService.GetById(id) != null;
        }
    }
}
