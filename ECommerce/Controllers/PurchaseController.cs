using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Entity.EntityClass;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Client;
using Repository.Interface;
using Repository.UnitOfWork;
using System.Collections.Generic;
using System.Security.Claims;
using Utility;

namespace ECommerce.Controllers
{
    [Authorize]
    public class PurchaseController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> signInManager;
        private readonly INotyfService _notyf;
        private readonly IUnitOfWork unitofwork;
        private readonly IGenericRepository<Card> cardRepository;
        private readonly IGenericRepository<Basket> basketRepository;
        private readonly IGenericRepository<Book> bookRepository;
        public PurchaseController(
            UserManager<AppUser> _userManager, IMapper mapper, 
            SignInManager<AppUser> signInManager, INotyfService _notyf,
            IUnitOfWork unitofwork
            )
        {
            this._userManager = _userManager;
            this._mapper = mapper;
            this.signInManager = signInManager;
            this._notyf = _notyf;
            this.unitofwork = unitofwork;
            this.cardRepository = unitofwork.GetRepository<Card>();
            this.basketRepository = unitofwork.GetRepository<Basket>();
            this.bookRepository = unitofwork.GetRepository<Book>();
        }
        [HttpPost]
        public async Task<IActionResult> AddToBasket([FromBody] PurchaseRequestModel purchaseRequest)
        {
            if(purchaseRequest.Count <= 0)
            {
                return BadRequest(); // 400 Bad Request
            }
            try
            {
                // implement logic card - basket logic
                string userid = User.GetLoggedInUserId<string>();
                Basket? basket = basketRepository.Find(x => x.UserId == userid && x.status == Entity.Enum.BasketStatus.Pending , x => x.Cards).FirstOrDefault();
                Book book = bookRepository.GetById(purchaseRequest.BookId);
                if (basket == null)
                {
                    basket = new Basket()
                    {
                        UserId = userid,
                        status = Entity.Enum.BasketStatus.Pending,
                        TotoalPrice = 0,
                        Cards = new List<Card>(),
                    };
                    basketRepository.Add(basket);
                    unitofwork.Commit(); // crete basket to get id 
                }
                // loking for cards
                bool flag = true;
                List<Card> cards = new List<Card>();
                cards.AddRange(basket.Cards);
                foreach (var card in cards)
                {
                    if(card.BookId == purchaseRequest.BookId)
                    {
                        card.BookCount += purchaseRequest.Count;
                        card.TotalPrice = CalculateTotalBookPrice(book, card.BookCount);
                        flag = false;
                        break;
                    }
                }
                // means there is no card for spesific book 
                if (flag)
                {
                    Card newcard = new Card()
                    {
                        BookCount = purchaseRequest.Count,
                        TotalPrice = CalculateTotalBookPrice(book, purchaseRequest.Count),
                        BasketId = basket.Id,
                        BookId = purchaseRequest.BookId,
                    };
                    cards.Add(newcard);
                    cardRepository.Add(newcard);
                }
                basket.TotoalPrice = CalculateBasketTotalPrice(cards);
                unitofwork.Commit();
                return Json(new { result = "success"});
            }
            catch (Exception ex)
            {
                var errorResponse = new { error = "An error occurred", message = ex.Message };
                return BadRequest(errorResponse); // 400 Bad Request
            }
        }
    
        public IActionResult Basket()
        {
            List<Card> model = new List<Card>(); 
            string userid = User.GetLoggedInUserId<string>();
            Basket? basket = basketRepository.Find(x => x.UserId == userid && x.status == Entity.Enum.BasketStatus.Pending, x => x.Cards).FirstOrDefault();
            if (basket != null)
            {
                foreach (var card in basket.Cards)
                {
                    model.Add(cardRepository.GetById(card.Id , x => x.Book));
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult AddSingleBook(int bookId)
        {
            try
            {
                string userid = User.GetLoggedInUserId<string>();
                Basket? basket = basketRepository.Find(x => x.UserId == userid && x.status == Entity.Enum.BasketStatus.Pending, x => x.Cards).FirstOrDefault();
                Book book = bookRepository.GetById(bookId);
                foreach (var card in basket.Cards)
                {
                    if (card.BookId == bookId)
                    {
                        card.BookCount++;
                        card.TotalPrice = CalculateTotalBookPrice(book, card.BookCount);
                        break;
                    }
                }
                unitofwork.Commit();
                _notyf.Success(SD.BookAddedToDatabase);
            }
            catch(Exception e)
            {
                _notyf.Error(SD.SomethingWentWrong);
            }
            return RedirectToAction("Basket");  
        }
        [HttpPost]
        public IActionResult RemoveSingleBook(int bookId)
        {
            try
            {
                string userid = User.GetLoggedInUserId<string>();
                Basket? basket = basketRepository.Find(x => x.UserId == userid && x.status == Entity.Enum.BasketStatus.Pending, x => x.Cards).FirstOrDefault();
                Book book = bookRepository.GetById(bookId);
                foreach (var card in basket.Cards)
                {
                    if (card.BookId == bookId)
                    {
                        card.BookCount--;
                        if(card.BookCount == 0)
                        {
                            cardRepository.Remove(card);
                            break;
                        }
                        card.TotalPrice = CalculateTotalBookPrice(book, card.BookCount);
                        break;
                    }
                }
                unitofwork.Commit();
                _notyf.Success(SD.BookRemovedFromDatabase);
            }
            catch (Exception e)
            {
                _notyf.Error(SD.SomethingWentWrong);
            }
            return RedirectToAction("Basket");
        }

        // helper methods
        public static double CalculateTotalBookPrice(Book book, int BookCount)
        {
            double total = 0;
            if (BookCount < 5) total = book.Price * BookCount;
            else if (BookCount < 10) total += book.Price5 * BookCount;
            else if (BookCount < 20) total += book.Price10 * BookCount;
            else total += book.Price20 * BookCount;
            if(book.DiscountRate != 0)
            {
                double rate = 100 - book.DiscountRate;
                total = total * rate / 100;
            }
            return total;
        }
        
        public static double CalculateBasketTotalPrice(IEnumerable<Card> list)
        {
            double total = 0;
            foreach (var card in list)
            {
                total += card.TotalPrice;    
            }
            return total;
        }
    }
}
