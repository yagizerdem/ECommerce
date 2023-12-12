using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class PurchaseController : Controller
    {
        public IActionResult AddToBasket()
        {
            return View();
        }
    }
}
