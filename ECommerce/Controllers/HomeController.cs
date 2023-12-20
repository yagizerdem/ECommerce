using Entity.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Repository.UnitOfWork;
using System.Diagnostics;
using System.Security.Claims;
using Utility;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitofwork;
        private readonly IGenericRepository<Book> bookRepository;
        private readonly IGenericRepository<UserProfile> userProfileRepository;
        public HomeController(ILogger<HomeController> logger , UserManager<AppUser> _userManager , IUnitOfWork unitofwork)
        {
            _logger = logger;
            this._userManager = _userManager;
            this.unitofwork = unitofwork;
            this.bookRepository = unitofwork.GetRepository<Book>();
            this.userProfileRepository = unitofwork.GetRepository<UserProfile>();
        }

        public async Task<IActionResult> Index()
        {            
            var userId = User.GetLoggedInUserId<string>(); // extension method 
            if(userId != null)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);

                // If the user has roles, you can use them (assuming a user has only one role in this example)
                if (roles.Any())
                {
                    var roleName = roles.First(); // Assuming a user has only one role                          // Now, roleName contains the name of the role for the current user
                    ViewBag.UserRole = roleName;
                }
            }
            // get books 
            IEnumerable<Book> book_list = bookRepository.GetAll();

            // finding user proile 
            UserProfile? userProfile = userProfileRepository.Find(x => x.AppUserId == userId , x=>x.AppUser).FirstOrDefault();
            if(userProfile != null)
            {
                ViewBag.userProfile = userProfile;
            }
            else
            {
                ViewBag.userProfile = null;
            }
            return View(book_list);
        }

        public IActionResult DetailsPage(int bookid)
        {
            Book book = bookRepository.GetById(bookid , x => x.Images);
            return View(book);
        }
        public IActionResult Comments()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }


        // for caht

        [HttpPost]
        public IActionResult HandleChatInput(string inputData)
        {
            if(inputData == null ||  inputData != null && inputData.Trim() == "" )
            {
                return Json(new { success = false, responseText = "Enter valid chat data" });
            }



            return Json(new { success = true, responseText = "Your message successfuly sent!" });
        }



    }
}
