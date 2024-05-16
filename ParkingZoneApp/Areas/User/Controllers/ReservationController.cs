using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.Security.Claims;

namespace ParkingZoneApp.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IParkingZoneService _parkingZoneService;
        private readonly IParkingSlotService _parkingSlotService;
        public ReservationController(
            IReservationService reservationService,
            IParkingZoneService parkingZoneService,
            IParkingSlotService parkingSlotService)
        {
            _reservationService = reservationService;
            _parkingZoneService = parkingZoneService;
            _parkingSlotService = parkingSlotService;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId.IsNullOrEmpty())
                return NotFound();

            var reservations = _reservationService.GetReservationsByUser(userId);
            var indexVM = IndexVM.MapToVM(reservations, _parkingZoneService, _parkingSlotService);
            return View(indexVM);
        }
    }
}
