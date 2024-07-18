using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ParkingSlots;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;

namespace ParkingZoneApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ParkingSlotController : Controller
    {
        private readonly IParkingSlotService _parkingSlotService;
        public ParkingSlotController(IParkingSlotService parkingSlotService)
        {
            _parkingSlotService = parkingSlotService;
        }

        public async Task<IActionResult> Index(Guid zoneId, [FromServices] IParkingZoneService _parkingZoneService)
        {
            var parkingSlots = await _parkingSlotService.GetSlotsByZoneIdAsync(zoneId);
            var listItemVMs = ListItemVM.MapToVM(parkingSlots).ToList();
            var zone = await _parkingZoneService.GetById(zoneId);
            ViewData["parkingZoneId"] = zoneId;
            ViewData["name"] = zone?.Name;
            return View(listItemVMs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(FilterSlotVM vm)
        {
            var slotsQuery = await _parkingSlotService.FilterAsync(vm);
            var listItemVm = ListItemVM.MapToVM(slotsQuery);
            return PartialView("_FilteredSlotsPartial", listItemVm);
        }

        [HttpGet]
        public IActionResult Create(Guid parkingZoneId)
        {
            CreateVM createVM = new()
            {
                ParkingZoneId = parkingZoneId,
            };

            return View(createVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVM slotCreateVM)
        {
            if (await _parkingSlotService.IsUniqueNumberAsync(slotCreateVM.ParkingZoneId, slotCreateVM.Number))
            {
                ModelState.AddModelError("Number", "The parking slot number is not unique");
            }

            if (ModelState.IsValid)
            {
                var parkingSlot = slotCreateVM.MapToModel();
                await _parkingSlotService.Insert(parkingSlot);
                return RedirectToAction("Index", new { zoneId = parkingSlot.ParkingZoneId });
            }
            return View(slotCreateVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var slot = await _parkingSlotService.GetById(id);

            if (slot is null)
                return NotFound();

            var editVM = new EditVM(slot);
            return View(editVM);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditVM slotEditVM, Guid id)
        {
            if (id != slotEditVM.Id)
                return NotFound();

            var slot = await _parkingSlotService.GetById(id);
            if (slot is null)
                return NotFound();

            if (await _parkingSlotService.IsUniqueNumberAsync(slotEditVM.ParkingZoneId, slotEditVM.Number) &
                slot.Number != slotEditVM.Number)
            {
                ModelState.AddModelError("Number", "The parking slot number is not unique!");
            }

            if (slotEditVM.IsSlotInUse & slotEditVM.Category != slot.Category)
                ModelState.AddModelError("Category", "This slot is in use, category cannot be modified!");

            if (ModelState.IsValid)
            {
                slot = slotEditVM.MapToModel(slot);
                await _parkingSlotService.Update(slot);
                return RedirectToAction("Index", new { zoneId = slot.ParkingZoneId });
            }

            return View(slotEditVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var slot = await _parkingSlotService.GetById(id);

            if (slot is null)
                return NotFound();

            DetailsVM detailsVM = new(slot);
            return View(detailsVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var slot = await _parkingSlotService.GetById(id);

            if (slot is null)
                return NotFound();

            return View(slot);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var existingParkingSlot = await _parkingSlotService.GetById(id);
            if (existingParkingSlot is null)
                return NotFound();

            if (existingParkingSlot.IsInUse)
                ModelState.AddModelError("DeleteButton", "This slot is in use, cannot be deleted!");
            else
                await _parkingSlotService.Remove(existingParkingSlot);

            return RedirectToAction("Index", new { zoneId = existingParkingSlot.ParkingZoneId });
        }
    }
}
