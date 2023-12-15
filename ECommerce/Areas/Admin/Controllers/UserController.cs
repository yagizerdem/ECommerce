using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Entity.EntityClass;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.UnitOfWork;
using Utility;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitofwork;
        private readonly INotyfService _notyf;
        private readonly IMapper _mapper;
        public UserController(UserManager<AppUser> _userManager , IUnitOfWork unitOfWork
            , SignInManager<AppUser> _signInManager , RoleManager<IdentityRole> _roleManager,
            INotyfService _notyf , IMapper mapper)
        {
            this._userManager = _userManager;   
            this._unitofwork = unitOfWork;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;
            this._notyf = _notyf;
            this._mapper = mapper;
        }
        public IActionResult List()
        {
            // fetchin user data from api 
            return View();
        }
        public async Task<IActionResult> AddUser() 
        {
            List<IdentityRole> roles = _roleManager.Roles.ToList();
            var items = new List<SelectListItem>();
            foreach (var role in roles)
            {
                if (role.Name.ToLower() == "admin") continue;
                items.Add(new SelectListItem { Value = role.Name, Text = role.Name });
            }
            ViewData["UserRoleOptions"] = items;

            SingInViewModel model = new SingInViewModel();
            return View(model);
        }
        [HttpPost]
        [ActionName("AddUser")]
        public async Task<IActionResult> AddUserToDatabase(SingInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                AppUser newuser = _mapper.Map<AppUser>(model);
                newuser.UserName = newuser.FirstName + "-" + newuser.LastName;
                await _userManager.CreateAsync(newuser, model.Password);
                await _userManager.AddToRoleAsync(newuser, model.UserRole);
                _unitofwork.Commit();
            }
            catch (Exception ex)
            {
                _notyf.Error(SD.UserAddError);
                return RedirectToAction("List", "User");
            }
            _notyf.Success(SD.UserAddSuccessfull);
            return RedirectToAction("List" , "User");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                AppUser user = await _userManager.FindByIdAsync(userId);
                await _userManager.DeleteAsync(user);
                return Json(new { success = true, message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return Json(new { success = false, message = "Error deleting user", error = ex.Message });
            }
        }
    }
}
