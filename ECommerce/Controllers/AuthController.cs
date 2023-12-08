using Entity.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Entity.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using Utility;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using AspNetCoreHero.ToastNotification.Abstractions;
namespace ECommerce.Controllers
{

    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> signInManager;
        private readonly INotyfService _notyf;
        public AuthController(UserManager<AppUser> _userManager, IMapper mapper, SignInManager<AppUser> signInManager , INotyfService _notyf)
        {
            this._userManager = _userManager;
            this._mapper = mapper;
            this.signInManager = signInManager;
            this._notyf = _notyf;
        }
        public IActionResult SignIn()
        {
            SingInViewModel model = new Entity.Models.SingInViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SingInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            AppUser appUser = _mapper.Map<AppUser>(model);
            appUser.UserName = appUser.FirstName + "-" + appUser.LastName;
            try
            {
                var result =  await _userManager.CreateAsync(appUser,model.Password);
                await _userManager.AddToRoleAsync(appUser, "Customer");
                if (!result.Succeeded)
                {
                    IEnumerable<IdentityError> errors = result.Errors.ToList();
                    foreach(var err in errors)
                    {
                        ModelState.AddModelError(err.Code , err.Description.ToString());
                    }
                    _notyf.Error(SD.SingINErrorMessage, 4);
                    return View(model);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            _notyf.Success(SD.SingInMessage, 4);
            return RedirectToAction("Index" , "Home");
        }

        public IActionResult LogIn()
        {
            LogInViewModel model = new LogInViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel model)
        {
            try
            {
                AppUser appuser = await _userManager.FindByEmailAsync(model.Email);
                if(appuser != null)
                {
                    var result = await signInManager.PasswordSignInAsync(appuser , model.Password , model.RememberMe, false);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("CustomError", "LogIn failed");
                        _notyf.Error(SD.LogInErrorMessage, 4);
                        return View(model);
                    }
                    // succesfull login
                    _notyf.Success(SD.LoggInMessage, 4);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            _notyf.Error(SD.LogInErrorMessage, 4);
            ModelState.AddModelError("CustomError", "User Not found");
            return View(model);

        }
        
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            _notyf.Information(SD.LogOutMessage, 4);
            return RedirectToAction("Index", "Home");    
        }

    }
}
