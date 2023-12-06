using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
