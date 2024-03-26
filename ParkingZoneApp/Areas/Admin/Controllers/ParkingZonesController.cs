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

        private readonly IRepository<ParkingZone> genericRepository;
        public ParkingZonesController(IRepository<ParkingZone> parkingZoneRepository)
        {
            genericRepository = parkingZoneRepository;
        }

        // GET: Admin/ParkingZones
        public ActionResult Index()
        {
            var entity = genericRepository.GetAll();
            return View(entity);
        }

        // GET: Admin/ParkingZones/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var parkingZone = genericRepository.GetByID(id);

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
                genericRepository.Add(parkingZone);
                return RedirectToAction(nameof(Index));
            }

            return View(parkingZone);
        }

        // GET: Admin/ParkingZones/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var parkingZone =genericRepository.GetByID(id);

            if (parkingZone == null)
                return NotFound();

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
                    genericRepository.Update(parkingZone);
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
            var parkingZone = genericRepository.GetByID(id);

            if (parkingZone == null)
                return NotFound();
            else
                genericRepository.Delete(id);

            return View(parkingZone);
        }

        // POST: Admin/ParkingZones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var parkingZone = genericRepository.GetByID(id);

            if (parkingZone != null)
            {
                genericRepository.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ParkingZoneExists(Guid id)
        {
            return genericRepository.GetByID(id) != null;
        }
    }
}
