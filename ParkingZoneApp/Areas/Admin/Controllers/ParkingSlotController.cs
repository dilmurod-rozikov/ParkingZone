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
        public ParkingSlotController(IParkingSlotService parkingSlotService)
        {
            _parkingSlotService = parkingSlotService;
        }

        public IActionResult Index(Guid zoneId)
        {
            var parkingSlots = _parkingSlotService.GetSlotsByZoneId(zoneId);
            var listItemVMs = ListItemVM.MapToVM(parkingSlots).ToList();
            var sorted = listItemVMs.OrderBy(item => item.Number);
            ViewData["parkingZoneId"] = zoneId;
            return View(sorted);
        }

        [HttpGet]
        public IActionResult Create(Guid parkingZoneId)
        {
            CreateVM createVM = new CreateVM()
            {
                ParkingZoneId = parkingZoneId
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
    }
}
