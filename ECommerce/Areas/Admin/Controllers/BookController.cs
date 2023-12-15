using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Entity.EntityClass;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Repository.Interface;
using Repository.UnitOfWork;
using System.Drawing;
using System.Net;
using Utility;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
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
        public IActionResult DeleteBook(int bookid)
        {
            try
            {
                Book deletedbook = bookRepository.GetById(bookid);
                bookRepository.Remove(deletedbook);
                unitofwork.Commit();
            }
            catch(Exception ex){
                _notyf.Information(SD.SomethingWentWrong);
                return View();
            }
            _notyf.Information(SD.BookRemovedFromDatabase);
            return RedirectToAction("List", "Book");
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
            if (User.IsInRole("Employee"))
            {
                return RedirectToAction("Index", "Employee" , new { area = "Employee" });
            }
            return RedirectToAction("Index" ,"Admin");
        }

        
        public IActionResult UpdateBook(int bookid)
        {
            Book book = bookRepository.GetById(bookid , x => x.Images);
            BookModel model = _mapper.Map<BookModel>(book); 
            List<string> imgpaths = new List<string>();
            imgpaths.Add(book.HeaderImagePath); 
            foreach (var img in book.Images)
            {
                imgpaths.Add(img.Path);
            }
            ViewBag.ImagePaths = imgpaths;
            TempData["headerimgurl"] = book.HeaderImagePath;
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateBook(BookModel model)
        {
            if (!ModelState.IsValid)
            {
                _notyf.Error(SD.EnterValidData);
                return View(model);
            }
            try
            {
                Book book = _mapper.Map<Book>(model);
                book.HeaderImagePath = (string)TempData["headerimgurl"];
                bookRepository.Update(book);
                Book updatedbook = bookRepository.GetById(book.Id);

                if (model.HeaderImage != null)
                {
                    // delting old header img
                    if (book.HeaderImagePath != null)
                    {
                        ImageDeleteHelper.RemoveImage(book.HeaderImagePath, webhostenv);
                    }
                    List<IFormFile> files = new();
                    files.Add(model.HeaderImage);
                    List<string> imgPaths = ImageDownloadHelper.SaveFile(files, model.Author + "-" + model.Title, webhostenv);
                    book.HeaderImagePath = imgPaths[0];
                }
                // saving sub images
                if (model.SubImages != null && model.SubImages.Count() != 0)
                {
                    List<IFormFile> files = new List<IFormFile>();
                    files.AddRange(model.SubImages);
                    List<string> imgPaths = ImageDownloadHelper.SaveFile(files, model.Author + "-" + model.Title, webhostenv); // sved to file system
                    foreach (var path in imgPaths)
                    {
                        Entity.EntityClass.Images img = new Entity.EntityClass.Images()
                        {
                            Path = path,
                            BookId = book.Id,
                        };
                        imagesRepository.Add(img);
                    }
                }

                unitofwork.Commit();
            }
            catch (Exception ex)
            {
                _notyf.Success(SD.SomethingWentWrong);
                return View(model);
            }

            _notyf.Success(SD.BookUpdatedSuccessfull);
            return RedirectToAction(nameof(List));
        }


        [HttpPost]
        public IActionResult DeleteImage(int bookid , string imgpath)
        {
            Book book = bookRepository.GetById(bookid, x => x.Images);
            try
            {
                if (imgpath == book.HeaderImagePath)
                {
                    // deleting header img from file system
                    ImageDeleteHelper.RemoveImage(imgpath , webhostenv);
                    book.HeaderImagePath = null;
                }
                // chkecing img path in sub imgages 
                foreach (var img in book.Images)
                {
                    if (img.Path == imgpath)
                    {
                        // deleting header img from file system
                        ImageDeleteHelper.RemoveImage(imgpath , webhostenv);
                        imagesRepository.Remove(img);
                    }
                }
                unitofwork.Commit();
            }
            catch (Exception ex)
            {
                _notyf.Error(SD.SomethingWentWrong);
                return RedirectToAction(nameof(UpdateBook) , new { bookid = book.Id });

            }
            _notyf.Success(SD.DeleteBookImageSuccessfull);
            return RedirectToAction(nameof(UpdateBook), new { bookid = book.Id });
        }
    }
}
