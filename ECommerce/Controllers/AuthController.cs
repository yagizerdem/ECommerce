using Entity.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{

    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        public AuthController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SingIn()
        {
            ;
            return RedirectToAction("Index" , "Home");
        }
    }
}
