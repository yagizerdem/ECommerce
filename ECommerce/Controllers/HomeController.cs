using Entity.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger , UserManager<AppUser> _userManager)
        {
            _logger = logger;
            this._userManager = _userManager;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.Email); // will give the user's userId
            if(userId != null)
            {
                var userName = User.Identity.Name;

                ViewBag.UserName = userName;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
