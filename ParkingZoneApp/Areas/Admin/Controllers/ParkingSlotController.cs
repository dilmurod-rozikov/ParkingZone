using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ParkingSlots;

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

        public IActionResult Index()
        {
            var parkingSlots = _parkingSlotService.GetAllParkingSlots();
            var listItemVMs = ListItemVM.MapToVM(parkingSlots);
            return View(listItemVMs);
        }
    }
}
