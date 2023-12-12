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
                // finding if card exit
                var userId = User.GetLoggedInUserId<string>();
                Card? cardFromDb = cardRepository.Find(x => x.BookId == purchaseRequest.BookId && x.AppUserId == userId).FirstOrDefault();
                Book book = bookRepository.GetById(purchaseRequest.BookId);
                if (cardFromDb == null)
                {
                    cardFromDb = new Card()
                    {
                        AppUserId = userId,
                        BookId = purchaseRequest.BookId,
                        BookCount = purchaseRequest.Count,
                        TotalPrice = CalculateTotalBookPrice(book, purchaseRequest.Count),
                    };
                    cardRepository.Add(cardFromDb);
                }
                else
                {
                    cardFromDb.BookCount += purchaseRequest.Count;
                    cardFromDb.TotalPrice = CalculateTotalBookPrice(book, cardFromDb.BookCount);
                }
                // finding basket 
                Basket? basketFromdb = basketRepository.Find(x => x.UserId == userId &&
                x.status == Entity.Enum.BasketStatus.Pending, x => new string[] { "Card" }).FirstOrDefault();
                if (basketFromdb == null)
                {
                    basketFromdb = new Basket()
                    {
                        UserId = userId,
                        status = Entity.Enum.BasketStatus.Pending,
                        TotoalPrice = cardFromDb.TotalPrice,
                    };
                    basketRepository.Add(basketFromdb);
                }
                else
                {
                    basketFromdb.TotoalPrice = CalculateBasketTotalPrice(basketFromdb.Cards);
                }
                // commiting changes to datbase
                unitofwork.Commit();
            }
            catch (Exception ex)
            {
                var errorResponse = new { error = "An error occurred", message = ex.Message };
                return BadRequest(errorResponse); // 400 Bad Request
            }
            return Json(new { result = "success" });
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
