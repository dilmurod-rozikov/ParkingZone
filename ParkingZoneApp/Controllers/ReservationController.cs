using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ReservationVMs;

namespace ParkingZoneApp.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IParkingZoneService _parkingZoneService;
        private readonly IParkingSlotService _parkingSlotService;
        private readonly IReservationService _reservationService;
        public ReservationController(
            IParkingZoneService parkingZoneService,
            IReservationService reservationService,
            IParkingSlotService parkingSlotService)
        {
            _parkingZoneService = parkingZoneService;
            _reservationService = reservationService;
            _parkingSlotService = parkingSlotService;
        }

        public IActionResult FreeSlots()
        {   
            var zones = _parkingZoneService.GetAll().ToList();
            FreeSlotsVMs freeSlotsVMs = new(zones);
            return View(freeSlotsVMs);
        }

        [HttpPost]
        public IActionResult FreeSlots(FreeSlotsVMs freeSlotsVMs)
        {
            var zones = _parkingZoneService.GetAll().ToList();
            freeSlotsVMs.ListOfZones = new SelectList(zones, "Id", "Name");
            freeSlotsVMs.ParkingSlots = _parkingSlotService.GetAllFreeSlots(freeSlotsVMs.SelectedZoneId);
            return View(freeSlotsVMs);
        }
    }
}
