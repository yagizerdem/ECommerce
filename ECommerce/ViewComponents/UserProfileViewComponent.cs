using Azure.Core;
using Entity.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Repository.Repository;
using Repository.UnitOfWork;

namespace ECommerce.ViewComponents
{
    public class UserProfileViewComponent : ViewComponent
    {
        private readonly IUnitOfWork unitofwork;
        private readonly IGenericRepository<UserProfile> userProfileRepository;
        private readonly UserManager<AppUser> _userManager;
        public UserProfileViewComponent(IUnitOfWork unitofwork, UserManager<AppUser> userManager)
        {
            this.userProfileRepository = unitofwork.GetRepository<UserProfile>();
            _userManager = userManager;

        }
        public IViewComponentResult Invoke()
        {
            string? userid = _userManager.GetUserId(Request.HttpContext.User);
            UserProfile? userProfile = userProfileRepository.Find(x => x.AppUserId == userid, x => x.AppUser).FirstOrDefault();
            if (userProfile != null)
            {
                ViewBag.userProfile = userProfile;
            }
            else
            {
                ViewBag.userProfile = null;
            }

            return View("UserProfile");
        }

    }
}
