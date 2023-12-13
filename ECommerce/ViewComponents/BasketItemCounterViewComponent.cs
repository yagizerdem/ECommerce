using Entity.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Repository.Repository;
using Repository.UnitOfWork;

namespace ECommerce.ViewComponents
{
    public class BasketItemCounterViewComponent : ViewComponent
    {
        private readonly IUnitOfWork unitofwork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Basket> basketRepository;
        public BasketItemCounterViewComponent(IUnitOfWork unitofwork, UserManager<AppUser> _userManager)
        {
            this.unitofwork = unitofwork;
            this._userManager = _userManager;
            this.basketRepository = unitofwork.GetRepository<Basket>();
        }
        public IViewComponentResult Invoke()
        {
            // Your logic here
            string? userid = _userManager.GetUserId(Request.HttpContext.User);
            Basket? basketfromdb = null;    
            if(userid != null)
            {
                basketfromdb = basketRepository.Find(x => x.UserId == userid && x.status == Entity.Enum.BasketStatus.Pending , x => x.Cards).FirstOrDefault();
            }
            if (basketfromdb != null)
                ViewBag.cards = basketfromdb.Cards.Count() == 0 ? " " : basketfromdb.Cards.Count().ToString();
            else ViewBag.cards = " ";
            return View("BasketItemCounter"); // The view name is inferred based on the ViewComponent name
        }
    }
}
