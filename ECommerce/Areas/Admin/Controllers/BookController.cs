using AspNetCoreHero.ToastNotification.Abstractions;
using Entity.EntityClass;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Repository.UnitOfWork;
using Utility;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork unitofwork;
        private readonly IGenericRepository<Book> bookRepository;
        private readonly INotyfService _notyf;
        public BookController(IUnitOfWork unitOfWork , INotyfService _notyf)
        {
            this.unitofwork = unitOfWork;
            this.bookRepository = unitofwork.GetRepository<Book>();
            this._notyf = _notyf;
        }
        public IActionResult List()
        {
            List<Book> books = bookRepository.GetAll().ToList();
            return View(books);
        }

        public IActionResult AddBook()
        {
            BookModel model = new BookModel();
            return View(model);
        }
        [HttpPost]
        [ActionName("AddBook")]
        public IActionResult AddBookToDatabase(BookModel model)
        {
            if (!ModelState.IsValid)
            {
                _notyf.Error(SD.BookModelErrorMessage, 4);
                return View(model);
            }
            // validate images
            List<IFormFile> files = model.SubImages;
            files.Add(model.HeaderImage);
            bool flag = ImageValidator.Control(files);
            if(flag == false)
            {
                _notyf.Error(SD.FileExtensionsNotCorrect, 4);
                return View(model);
            }


            _notyf.Success(SD.BookAddedToDatabase, 4);
            return View();
        }
    }
}
