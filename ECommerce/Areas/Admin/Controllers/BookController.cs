using Entity.EntityClass;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Repository.UnitOfWork;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork unitofwork;
        private readonly IGenericRepository<Book> bookRepository;
        public BookController(IUnitOfWork unitOfWork)
        {
            this.unitofwork = unitOfWork;
            this.bookRepository = unitofwork.GetRepository<Book>();
        }
        public IActionResult List()
        {
            List<Book> books = bookRepository.GetAll().ToList();
            return View(books);
        }

        public IActionResult AddBook()
        {
            return View();
        }
    }
}
