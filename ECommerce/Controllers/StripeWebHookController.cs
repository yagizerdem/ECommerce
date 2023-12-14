using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Repository.UnitOfWork;
using Utility;

namespace ECommerce.Controllers
{
    public class StripeWebHookController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IUnitOfWork unitofwork;
        public StripeWebHookController(INotyfService _notyf , IUnitOfWork unitofwork)
        {
            this._notyf = _notyf;
            this.unitofwork = unitofwork;
        }
        public IActionResult UpdateStatus(bool iSuccess) 
        {
            TempData["IsSuccessful"] = iSuccess;
            return View();
        }
        [HttpPost]
        [ActionName(nameof(UpdateStatus))]
        public IActionResult UpdatePaymentStatus()
        {
            bool isSuccessful = (bool)TempData["IsSuccessful"];
            if (isSuccessful == false)
            {
                _notyf.Error(SD.PaymentFailded);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
