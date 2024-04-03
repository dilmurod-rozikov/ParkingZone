﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Models;
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
            return View(new ListItemVM().MapToModel(parkingZones));
        }

        // GET: Admin/ParkingZones/Details/5
        public IActionResult Details(Guid? id)
        {
            var parkingZone = _parkingZoneService.GetById(id);

            if (parkingZone is null)
                return NotFound();

            return View(new DetailsVM().MapToModel(parkingZone));
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
                _parkingZoneService.Insert(new CreateVM().MapToModel(parkingZoneCreateVM));
                return RedirectToAction(nameof(Index));
            }

            return View(new CreateVM().MapToModel(parkingZoneCreateVM));
        }

        // GET: Admin/ParkingZones/Edit/5
        public IActionResult Edit(Guid? id)
        {
            var parkingZone = _parkingZoneService.GetById(id);

            if (parkingZone is null)
                return NotFound();

            _parkingZoneService.Update(parkingZone);
            return View(new EditVM().MapToModel(parkingZone));
        }

        // POST: Admin/ParkingZones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, ParkingZone parkingZone)
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
        
            return View(new EditVM().MapToModel(parkingZone));
        }

        // GET: Admin/ParkingZones/Delete/5
        public IActionResult Delete(Guid id)
        {
            var parkingZone = _parkingZoneService.GetById(id);

            if (parkingZone == null)
                return NotFound();

            return View(parkingZone);
        }

        // POST: Admin/ParkingZones/Delete/5
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
