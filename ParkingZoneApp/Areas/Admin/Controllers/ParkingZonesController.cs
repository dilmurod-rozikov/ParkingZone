using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Areas.Admin.Models;
using ParkingZoneApp.Data;

namespace ParkingZoneApp.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ParkingZonesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParkingZonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET:     
        public async Task<IActionResult> Index()
        {
            return View(await _context.ParkingZone.ToListAsync());
        }

        // GET: Admin/ParkingZones/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();
         
            var parkingZone = await _context.ParkingZone
                .FirstOrDefaultAsync(m => m.Id == id);

            if (parkingZone == null)
                return NotFound();

            return View(parkingZone);
        }

        // GET: Admin/ParkingZones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ParkingZones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParkingZone parkingZone)
        {

            if (ModelState.IsValid)
            {
                parkingZone.Id = Guid.NewGuid();
                _context.Add(parkingZone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(parkingZone);
        }

        // GET: Admin/ParkingZones/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();
            
            var parkingZone = await _context.ParkingZone.FindAsync(id);

            if (parkingZone == null)
                return NotFound();
            
            return View(parkingZone);
        }

        // POST: Admin/ParkingZones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        {
            if (id != parkingZone.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkingZone);
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var parkingZone = await _context.ParkingZone
                .FirstOrDefaultAsync(m => m.Id == id);

            if (parkingZone == null)
                return NotFound();
           
            return View(parkingZone);
        }

        // POST: Admin/ParkingZones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var parkingZone = await _context.ParkingZone.FindAsync(id);

            if (parkingZone != null)
                _context.ParkingZone.Remove(parkingZone);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingZoneExists(Guid id)
        {
            return _context.ParkingZone.Any(e => e.Id == id);
        }
    }
}
