using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Models;
using ParkingZoneApp.Repository.Interfaces;
namespace ParkingZoneApp.Areas.Admin
{
    [Area("Admin")]
    [Authorize]
    public class ParkingZonesController : Controller
    {

        private readonly IParkingZoneRepository _parkingZoneRepository;
        public ParkingZonesController(IParkingZoneRepository parkingZoneRepository)
        {
            _parkingZoneRepository = parkingZoneRepository;
        }

        // GET: Admin/ParkingZones
        public ActionResult Index()
        {
            var entity = _parkingZoneRepository.GetAll();
            return View(entity);
        }

        // GET: Admin/ParkingZones/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var parkingZone = _parkingZoneRepository.GetByID(id);

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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ParkingZone parkingZone)
        {
            if (ModelState.IsValid)
            {
                parkingZone.Id = Guid.NewGuid();
                _parkingZoneRepository.Add(parkingZone);
                return RedirectToAction(nameof(Index));
            }

            return View(parkingZone);
        }

        // GET: Admin/ParkingZones/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var parkingZone = _parkingZoneRepository.GetByID(id);

            if (parkingZone == null)
                return NotFound();
            _parkingZoneRepository.Update(parkingZone);

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
                    _parkingZoneRepository.Update(parkingZone);
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
            var parkingZone = _parkingZoneRepository.GetByID(id);

            if (parkingZone == null)
                return NotFound();
            else
                _parkingZoneRepository.Delete(id);

            return View(parkingZone);
        }

        // POST: Admin/ParkingZones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var parkingZone = _parkingZoneRepository.GetByID(id);

            if (parkingZone != null)
            {
                _parkingZoneRepository.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ParkingZoneExists(Guid id)
        {
            return _parkingZoneRepository.GetByID(id) != null;
        }
    }
}
