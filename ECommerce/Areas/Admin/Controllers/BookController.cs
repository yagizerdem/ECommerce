using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment webhostenv;
        private readonly IGenericRepository<Images> imagesRepository;

        public BookController(IUnitOfWork unitOfWork , INotyfService _notyf , IMapper _mapper , IWebHostEnvironment webhostenv )
        {
            this.unitofwork = unitOfWork;
            this.bookRepository = unitofwork.GetRepository<Book>();
            this.imagesRepository = unitofwork.GetRepository<Images>();
            this._notyf = _notyf;
            this._mapper = _mapper;
            this.webhostenv = webhostenv;
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
            List<IFormFile> files = new List<IFormFile>();
            if(model.SubImages != null || (model.SubImages != null && model.SubImages.Count() > 0)) files.AddRange(model.SubImages);
            if(model.HeaderImage != null) files.Add(model.HeaderImage);
            bool flag = ImageValidator.Control(files);
            if(flag == false)
            {
                _notyf.Error(SD.FileExtensionsNotCorrect, 4);
                return View(model);
            }
            // save book on database and img on file system
            Book book = _mapper.Map<Book>(model);
            // save img file here
            List<string> imgPaths = new();
            try
            {
                if (files != null && files.Count() > 0)
                {
                    imgPaths = ImageDownloadHelper.SaveFile(files, model.Author + "-" + model.Title, webhostenv);
                }
                // saving header img path on database
                if (imgPaths.Count() >= 1 && model.HeaderImage != null)
                {
                    book.HeaderImagePath = imgPaths[imgPaths.Count - 1];
                }
                // add book to database
                bookRepository.Add(book);
                unitofwork.Commit();
                // savin sub image paths to database on Images table
                if (imgPaths.Count() > 1)
                {
                    List<Images> subimglist = new();
                    // removinl last img path becouse last img path is header img not sub image !!!
                    var allpaths = model.HeaderImage != null ? imgPaths.Take(imgPaths.Count() - 1).ToList() : imgPaths.Take(imgPaths.Count()).ToList();
                    foreach (var path in allpaths)
                    {
                        Images images = new Images()
                        {
                            Path = path,
                            BookId = book.Id,
                        };
                        subimglist.Add(images);
                    }
                    imagesRepository.AddRange(subimglist);
                }
                unitofwork.Commit();
            }
            catch (Exception ex)
            {
                _notyf.Error(SD.InternalErrorOccured , 4);
                return View(model);
            }
            _notyf.Success(SD.BookAddedToDatabase, 4);
            return RedirectToAction("Index" ,"Admin");
        }
    }
}
