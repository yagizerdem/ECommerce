using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
