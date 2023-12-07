using Entity.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Entity.Models;
using AutoMapper;
namespace ECommerce.Controllers
{

    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        public AuthController(SignInManager<AppUser> signInManager , IMapper mapper)
        {
            _signInManager = signInManager;
            _mapper = mapper;
        }
        public IActionResult SignIn()
        {
            SingInViewModel model = new Entity.Models.SingInViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult SingIn(SingInViewModel model)
        {
            AppUser appUser = _mapper.Map<AppUser>(model);
            ;
            return RedirectToAction("Index" , "Home");
        }
    }
}
