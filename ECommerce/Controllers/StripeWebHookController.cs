using AspNetCoreHero.ToastNotification.Abstractions;
using Entity.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Interface;
using Repository.Repository;
using Repository.UnitOfWork;
using System.Runtime.CompilerServices;
using Utility;

namespace ECommerce.Controllers
{
    public class StripeWebHookController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IUnitOfWork unitofwork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Basket> _basketRepository;
        public StripeWebHookController(INotyfService _notyf , IUnitOfWork unitofwork,
            UserManager<AppUser> userManager)
        {
            this._notyf = _notyf;
            this.unitofwork = unitofwork;
            this._userManager = userManager;
            this._basketRepository = unitofwork.GetRepository<Basket>();
        }
        public IActionResult UpdateStatus(bool iSuccess) 
        {
            TempData["IsSuccessful"] = iSuccess;
            string? json = (string)TempData["OrderDetails"];
            TempData["ForwardOrderDetails"] = json;
            return View();
        }
        [HttpPost]
        [ActionName(nameof(UpdateStatus))]
        public IActionResult UpdatePaymentStatus()
        {
            string? json = (string)TempData["ForwardOrderDetails"];
            OrderDetails orderDetails = JsonConvert.DeserializeObject<OrderDetails>(json);

            bool isSuccessful = (bool)TempData["IsSuccessful"];
            if (isSuccessful == false)
            {
                _notyf.Error(SD.PaymentFailded);
                return RedirectToAction("Index", "Home");
            }
            // success paymentt
            string userid = User.GetLoggedInUserId<string>();
            Basket? basketfromdb = _basketRepository.Find(x => x.UserId == userid && x.status == Entity.Enum.BasketStatus.Pending).FirstOrDefault();
            //basketfromdb.status = Entity.Enum.BasketStatus.Approved;
            // creating new order

            _notyf.Success(SD.SuccessfulPaymetn);
            return RedirectToAction("Index", "Home"); ;
        }
    }
}
