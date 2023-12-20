using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Areas.Shipper.Controllers
{
    [Area("Shipper")]
    public class ShipperHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
