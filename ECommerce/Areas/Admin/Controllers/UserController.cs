using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Entity.EntityClass;
using Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.UnitOfWork;
using Utility;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            string currentuserid = User.GetLoggedInUserId<string>();
            List<AppUser> users = _userManager.Users.Where(x => x.Id != currentuserid).ToList();
            return View(users);
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
    }
}
