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
        public HomeController(ILogger<HomeController> logger , UserManager<AppUser> _userManager , IUnitOfWork unitofwork)
        {
            _logger = logger;
            this._userManager = _userManager;
            this.unitofwork = unitofwork;
            this.bookRepository = unitofwork.GetRepository<Book>();
        }

        public IActionResult Index()
        {
            var userId = User.GetLoggedInUserId<string>(); // extension method 
            if(userId != null)
            {
                var userName = User.Identity.Name;

                ViewBag.UserName = userName;

                // cheking user is admin
                ViewBag.IsAdmin  = User.IsInRole("Admin");
            }
            // get books 
            IEnumerable<Book> book_list = bookRepository.GetAll();
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

    }
}
