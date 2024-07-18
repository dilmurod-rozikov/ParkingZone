using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingZoneApp.ViewModels.PaymentVMs;
using ParkingZoneApp.Services.Interfaces;
using ParkingZoneApp.Models.Entities;

namespace ParkingZoneApp.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> MakePayment(Guid reservationId, [FromServices] IReservationService reservationService)
        {
            var reservation =  await reservationService.GetById(reservationId);
            if (reservation is null)
                return NotFound();

            PaymentVM paymentVM = new()
            {
                ParkingSlot = reservation.ParkingSlot,
            };

            return View(paymentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakePayment(PaymentVM paymentVM)
        {
            if (paymentVM is null)
                return NotFound();

            paymentVM.ParkingSlot = new();
            if (ModelState.IsValid)
            {
                var payment = paymentVM.MapToModel();
                bool result = await _paymentService.StorePaymentDetails(payment);
                if (!result)
                {
                    ModelState.AddModelError("", "Something went wrong while saving.");
                    return BadRequest();
                }

                TempData["SuccessMessage"] = "Payment was successful.";
            }

            return View(paymentVM);
        }
    }
}
