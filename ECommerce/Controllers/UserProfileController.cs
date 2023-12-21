using AspNetCoreHero.ToastNotification.Abstractions;
using Entity.EntityClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Repository.UnitOfWork;
using Utility;

namespace ECommerce.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserProfile> userProfileRepository;
        private readonly INotyfService _notyf;
        public UserProfileController(IUnitOfWork unitOfWork, INotyfService _notyf)
        {
            this._notyf = _notyf;
            this._unitOfWork = unitOfWork;  
            userProfileRepository = _unitOfWork.GetRepository<UserProfile>();
        }
        [HttpPost]
        public IActionResult CreateUserProfile()
        {
            try
            {
                var userId = User.GetLoggedInUserId<string>();
                UserProfile userProfile = new UserProfile()
                {
                    ProfileImgPath = null,
                    AppUserId = userId
                };
                userProfileRepository.Add(userProfile);
                _unitOfWork.Commit();
                _notyf.Success(SD.UserProfileCreatedSuccessfull);
            }
            catch (Exception ex)
            {
                _notyf.Error(SD.SomethingWentWrong);
            }
            return RedirectToAction("Index", "Home");
        }
        
        public IActionResult UserProfileHomePage()
        {
            ;
            return View();
        }

    }
}
