using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ParkingSlots;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;

namespace ParkingZoneApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ParkingSlotController : Controller
    {
        private readonly IParkingSlotService _parkingSlotService;
        private readonly IParkingZoneService _parkingZoneService;
        public ParkingSlotController(IParkingSlotService parkingSlotService, IParkingZoneService parkingZoneService)
        {
            _parkingSlotService = parkingSlotService;
            _parkingZoneService = parkingZoneService;
        }

        public IActionResult Index(Guid zoneId)
        {
            var parkingSlots = _parkingSlotService.GetSlotsByZoneId(zoneId);
            var listItemVMs = ListItemVM.MapToVM(parkingSlots).ToList();
            var zone = _parkingZoneService.GetById(zoneId);
            ViewData["parkingZoneId"] = zoneId;
            ViewData["name"] = zone?.Name;
            return View(listItemVMs);
        }

        [HttpPost]
        public IActionResult Index(FilterSlotVM vm)
        {
            var slotsQuery = _parkingSlotService.GetSlotsByZoneId(vm.ParkingZoneId);

            if (vm.Category.HasValue)
                slotsQuery = _parkingSlotService.FilterByCategory(slotsQuery, vm.Category);

            if (vm.IsSlotFree.HasValue)
                slotsQuery = _parkingSlotService.FilterByFreeSlot(slotsQuery, vm.IsSlotFree);
            var vms = ListItemVM.MapToVM(slotsQuery);
            return PartialView("_FilteredSlotsPartial", vms);
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
        public IActionResult Create(CreateVM slotCreateVM)
        {
            if (_parkingSlotService.IsUniqueNumber(slotCreateVM.ParkingZoneId, slotCreateVM.Number))
            {
                ModelState.AddModelError("Number", "The parking slot number is not unique");
            }

            if (ModelState.IsValid)
            {
                var parkingSlot = slotCreateVM.MapToModel();
                _parkingSlotService.Insert(parkingSlot);
                return RedirectToAction("Index", new { zoneId = parkingSlot.ParkingZoneId });
            }
            return View(slotCreateVM);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var slot = _parkingSlotService.GetById(id);

            if (slot is null)
                return NotFound();

            var editVM = new EditVM(slot);
            return View(editVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditVM slotEditVM, Guid id)
        {
            if (id != slotEditVM.Id)
                return NotFound();
            var slot = _parkingSlotService.GetById(id);

            if (slot is null)
                return NotFound();

            if (_parkingSlotService.IsUniqueNumber(slotEditVM.ParkingZoneId, slotEditVM.Number) & slot.Number != slotEditVM.Number)
            {
                ModelState.AddModelError("Number", "The parking slot number is not unique!");
            }

            if (slotEditVM.IsSlotInUse & slotEditVM.Category != slot.Category)
            {
                ModelState.AddModelError("Category", "This slot is in use, category cannot be modified!");
            }

            if (ModelState.IsValid)
            {
                slot = slotEditVM.MapToModel(slot);
                _parkingSlotService.Update(slot);
                return RedirectToAction("Index", new { zoneId = slot.ParkingZoneId });
            }

            return View(slotEditVM);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var slot = _parkingSlotService.GetById(id);

            if (slot is null)
                return NotFound();

            DetailsVM detailsVM = new(slot);
            return View(detailsVM);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var slot = _parkingSlotService.GetById(id);

            if (slot is null)
                return NotFound();

            return View(slot);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var existingParkingSlot = _parkingSlotService.GetById(id);
            if (existingParkingSlot is null)
                return NotFound();

            if (existingParkingSlot.IsInUse)
                ModelState.AddModelError("DeleteButton", "This slot is in use, cannot be deleted!");
            else
                _parkingSlotService.Remove(existingParkingSlot);

            return RedirectToAction("Index", new { zoneId = existingParkingSlot.ParkingZoneId });
        }
    }
}
