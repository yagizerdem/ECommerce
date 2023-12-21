using AspNetCoreHero.ToastNotification.Abstractions;
using Entity.EntityClass;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IWebHostEnvironment _webhostenv;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Entity.EntityClass.Comment> _commentRepository;
        private readonly IGenericRepository<Entity.EntityClass.Order> _orderRepository;
        public UserProfileController(IUnitOfWork unitOfWork, INotyfService _notyf , 
            IWebHostEnvironment _webhostenv , UserManager<AppUser> _userManager )
        {
            this._notyf = _notyf;
            this._unitOfWork = unitOfWork;  
            this.userProfileRepository = _unitOfWork.GetRepository<UserProfile>();
            this._webhostenv = _webhostenv;
            this._userManager = _userManager;
            this._commentRepository = _unitOfWork.GetRepository<Comment>();
            this._orderRepository = _unitOfWork.GetRepository<Order>();
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
            UserProfileModel model = new UserProfileModel();
            string userId = User.GetLoggedInUserId<string>();
            try
            {
                UserProfile? userProfile = userProfileRepository.Find(x => x.AppUserId == userId , x => x.AppUser).FirstOrDefault();
                if (userProfile == null)
                {
                    _notyf.Error(SD.SomethingWentWrong);
                    return RedirectToAction("Index", "Home");
                }
                model.userProfile = userProfile;
                List<Comment> comments = _commentRepository.Find(x => x.UserProfileId == userProfile.Id).ToList();
                List<Order> allOrders = _orderRepository.Find(x => x.UserId == userId, x => x.OrderDetails).ToList();
                model.comments = comments;
                model.orders = allOrders;

            }
            catch (Exception ex)
            {
                _notyf.Error(SD.SomethingWentWrong);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditProfilePhoto(int userProfileId , IFormFile file)
        {
            try
            {
                if(file == null)
                {
                    _notyf.Error("Enter Profile Image to form");
                    return RedirectToAction(nameof(UserProfileHomePage));
                }
                // folder name
                UserProfile userProfile = userProfileRepository.GetById(userProfileId , x=>x.AppUser);
                AppUser? user = await _userManager.FindByIdAsync(userProfile.AppUserId);

                string folderName = $"{user.Id}_{user.FirstName}_{user.LastName}";

                string path = UserProfileImgDownladHelper.SaveFile(file, folderName, _webhostenv);
                userProfile.ProfileImgPath = path;
                _unitOfWork.Commit();
                _notyf.Success("Success");
            }
            catch (Exception ex)
            {
                _notyf.Error(SD.SomethingWentWrong);
            }
            return RedirectToAction(nameof(UserProfileHomePage));
        }

        [HttpPost]
        public IActionResult DeleteChatMessage(int chatId)
        {
            try
            {
                Comment comment = _commentRepository.GetById(chatId);
                _commentRepository.Remove(comment);
                _unitOfWork.Commit();
                _notyf.Success("Comment Deleted Successfull");
            }
            catch(Exception ex) 
            {
                _notyf.Error(SD.SomethingWentWrong);
            }

            return RedirectToAction(nameof(UserProfileHomePage));
        }
    }
}
