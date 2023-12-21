using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Entity.EntityClass;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using NuGet.ContentModel;
using Repository.Interface;
using Repository.UnitOfWork;
using Stripe.Checkout;
using System.Net;
using Utility;

namespace ECommerce.Controllers
{
    [Authorize]
    public class PurchaseController : Controller
    {
        private readonly IWebHostEnvironment env;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> signInManager;
        private readonly INotyfService _notyf;
        private readonly IUnitOfWork unitofwork;
        private readonly IGenericRepository<Entity.EntityClass.Card> cardRepository;
        private readonly IGenericRepository<Basket> basketRepository;
        private readonly IGenericRepository<Book> bookRepository;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<UserProfile> _userProfileRepository;
        public PurchaseController(
            UserManager<AppUser> _userManager, IMapper mapper,
            SignInManager<AppUser> signInManager, INotyfService _notyf,
            IUnitOfWork unitofwork, IWebHostEnvironment env
            )
        {
            this._userManager = _userManager;
            this._mapper = mapper;
            this.signInManager = signInManager;
            this._notyf = _notyf;
            this.unitofwork = unitofwork;
            this.cardRepository = unitofwork.GetRepository<Entity.EntityClass.Card>();
            this.basketRepository = unitofwork.GetRepository<Basket>();
            this.bookRepository = unitofwork.GetRepository<Book>();
            this.env = env;
            this._orderRepository = unitofwork.GetRepository<Order>();
            this._userProfileRepository = unitofwork.GetRepository<UserProfile>();
        }
        [HttpPost]
        public async Task<IActionResult> AddToBasket([FromBody] PurchaseRequestModel purchaseRequest)
        {
            string userid = User.GetLoggedInUserId<string>();
            Basket? basket = basketRepository.Find(x => x.UserId == userid && x.status == Entity.Enum.BasketStatus.Pending, x => x.Cards).FirstOrDefault();


            // cheking book amoutn exeeds in database 
            if(basket != null)
            {
                foreach (var card in basket.Cards)
                {
                    Book bookfromdb = bookRepository.GetById(card.BookId);
                    if (bookfromdb.StockCount < card.BookCount)
                    {
                        return BadRequest(); // ajax ll handle the response and view 
                    }
                }
            }

            Book book = bookRepository.GetById(purchaseRequest.BookId);
            if (purchaseRequest == null  || (purchaseRequest != null && purchaseRequest.Count <= 0))
            {
                return BadRequest(); // 400 Bad Request
            }
            if (purchaseRequest.Count > book.StockCount)
            {
                return BadRequest(); // 400 Bad Request
            }
            try
            {
                // implement logic card - basket logic

                if (basket == null)
                {
                    basket = new Basket()
                    {
                        UserId = userid,
                        status = Entity.Enum.BasketStatus.Pending,
                        TotoalPrice = 0,
                        Cards = new List<Entity.EntityClass.Card>(),
                    };
                    basketRepository.Add(basket);
                    unitofwork.Commit(); // crete basket to get id 
                }
                // loking for cards
                bool flag = true;
                List<Entity.EntityClass.Card> cards = new List<Entity.EntityClass.Card>();
                cards.AddRange(basket.Cards);
                foreach (var card in cards)
                {
                    if (card.BookId == purchaseRequest.BookId)
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
                    Entity.EntityClass.Card newcard = new Entity.EntityClass.Card()
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
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                var errorResponse = new { error = "An error occurred", message = ex.Message };
                return BadRequest(errorResponse); // 400 Bad Request
            }
        }

        public IActionResult Basket()
        {
            List<Entity.EntityClass.Card> model = new List<Entity.EntityClass.Card>();
            string userid = User.GetLoggedInUserId<string>();
            Basket? basket = basketRepository.Find(x => x.UserId == userid && x.status == Entity.Enum.BasketStatus.Pending, x => x.Cards).FirstOrDefault();
            if (basket != null)
            {
                foreach (var card in basket.Cards)
                {
                    model.Add(cardRepository.GetById(card.Id, x => x.Book));
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


                // cheking book amoutn exeeds in database 
                foreach (var card in basket.Cards)
                {
                    Book bookfromdb = bookRepository.GetById(card.BookId);
                    if (bookfromdb.StockCount <= card.BookCount)
                    {
                        _notyf.Error(SD.EnterValidBookCount);
                        return RedirectToAction("Basket"); 
                    }
                }

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
            catch (Exception e)
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
                        if (card.BookCount == 0)
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

        public IActionResult Payment()
        {
            OrderFormModel model = new OrderFormModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Payment(OrderFormModel model)
        {
            var url = Request.Scheme + "://" + Request.Host.Value; // redirection url after payment
            OrderDetails orderDetails = _mapper.Map<OrderDetails>(model);
            string userid = User.GetLoggedInUserId<string>();
            Basket basketfromdb = basketRepository.Find(x => x.UserId == userid && x.status == Entity.Enum.BasketStatus.Pending , x => x.Cards).First();



            // creatgin list of books for paymetn
            List<Stripe.Checkout.SessionLineItemOptions> list = new List<Stripe.Checkout.SessionLineItemOptions>();
            foreach (var card in basketfromdb.Cards)
            {
                Book book = bookRepository.GetById(card.BookId);
                Stripe.Checkout.SessionLineItemOptions option = new Stripe.Checkout.SessionLineItemOptions()
                {
                    PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                        {
                            Name = card.Book.Title.ToString(),
                        },
                        UnitAmount = (int)ReturnUnitPrice(card.Book , card.BookCount)*100,
                    },
                    Quantity = card.BookCount,
                };
                list.Add(option);
            }

            // stripe config
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = list,
                Mode = "payment",
                SuccessUrl = Url.Action("UpdateStatus", "StripeWebHook", new { iSuccess  = true}, Request.Scheme),
                CancelUrl = Url.Action("UpdateStatus", "StripeWebHook", new { iSuccess = false}, Request.Scheme),
            };

            var service = new SessionService();
            Session session = service.Create(options);

            string json = JsonConvert.SerializeObject(orderDetails);
            TempData["OrderDetails"] = json;

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        // refuond book 
        [HttpPost]
        public IActionResult Refound(int OrderId)
        {
            double moneyRefound = 0;
            try
            {
                Order order = _orderRepository.GetById(OrderId , x=> x.OrderDetails);
                List<Entity.EntityClass.Basket> basketList = new List<Basket>();
                foreach (var orderdetail in order.OrderDetails)
                {
                    Basket basketFromdb = basketRepository.GetById(orderdetail.BasketId , x=>x.Cards);
                    basketList.Add(basketFromdb);
                    moneyRefound += basketFromdb.TotoalPrice;
                }
                foreach (var basket in basketList)
                {
                    foreach (Card cardFromdb in basket.Cards)
                    {
                        Book book = bookRepository.GetById(cardFromdb.BookId);
                        book.StockCount += cardFromdb.BookCount;
                    }
                }
                order.OrderStatus = Entity.Enum.OrderStatus.Refound;
                // refound mone 
                string UserId = User.GetLoggedInUserId<string>();
                UserProfile profile = _userProfileRepository.Find(x => x.AppUserId == UserId).First();
                profile.Wallet += moneyRefound;
                unitofwork.Commit();
                _notyf.Success("Successfully refound book");
            }
            catch (Exception ex)
            {
                _notyf.Error(SD.SomethingWentWrong);
            }
            return RedirectToAction("UserProfileHomePage" , "UserProfile");
        }

        [HttpPost]
        public  async Task<IActionResult> CancelOrder(int OrderId)
        {
            double moneyRefound = 0;
            try
            {
                Order order = _orderRepository.GetById(OrderId, x => x.OrderDetails);
                List<Entity.EntityClass.Basket> basketList = new List<Basket>();
                foreach (var orderdetail in order.OrderDetails)
                {
                    Basket basketFromdb = basketRepository.GetById(orderdetail.BasketId, x => x.Cards);
                    basketList.Add(basketFromdb);
                    moneyRefound += basketFromdb.TotoalPrice;
                }
                foreach (var basket in basketList)
                {
                    foreach (Card cardFromdb in basket.Cards)
                    {
                        Book book = bookRepository.GetById(cardFromdb.BookId);
                        book.StockCount += cardFromdb.BookCount;
                    }
                }
                order.OrderStatus = Entity.Enum.OrderStatus.Canceled;
                // refound mone 
                string UserId = User.GetLoggedInUserId<string>();
                UserProfile profile = _userProfileRepository.Find(x => x.AppUserId == UserId).First();
                profile.Wallet += moneyRefound;

                unitofwork.Commit();
                _notyf.Success("Successfully canceled book order");
            }
            catch (Exception ex)
            {
                _notyf.Error(SD.SomethingWentWrong);
            }
            return RedirectToAction("List", "Order", new {area="Admin"});
        }

        [HttpPost]
        public IActionResult BanOrder(int OrderId)
        {
            try
            {
                Order order = _orderRepository.GetById(OrderId, x => x.OrderDetails);
                List<Entity.EntityClass.Basket> basketList = new List<Basket>();
                foreach (var orderdetail in order.OrderDetails)
                {
                    Basket basketFromdb = basketRepository.GetById(orderdetail.BasketId, x => x.Cards);
                    basketList.Add(basketFromdb);
                }
                foreach (var basket in basketList)
                {
                    foreach (Card cardFromdb in basket.Cards)
                    {
                        Book book = bookRepository.GetById(cardFromdb.BookId);
                        book.StockCount += cardFromdb.BookCount;
                    }
                }
                order.OrderStatus = Entity.Enum.OrderStatus.Banned;
                unitofwork.Commit();
                _notyf.Success("Successfully banned book order");
            }
            catch (Exception ex)
            {
                _notyf.Error(SD.SomethingWentWrong);
            }
            return RedirectToAction("List", "Order", new { area = "Admin" });
        }


        // helper methods
        public static double CalculateTotalBookPrice(Book book, int BookCount)
        {
            double total = 0;
            if (BookCount < 5) total = book.Price * BookCount;
            else if (BookCount < 10) total += book.Price5 * BookCount;
            else if (BookCount < 20) total += book.Price10 * BookCount;
            else total += book.Price20 * BookCount;
            if (book.DiscountRate != 0)
            {
                double rate = 100 - book.DiscountRate;
                total = total * rate / 100;
            }
            return total;
        }

        public static double CalculateBasketTotalPrice(IEnumerable<Entity.EntityClass.Card> list)
        {
            double total = 0;
            foreach (var card in list)
            {
                total += card.TotalPrice;
            }
            return total;
        }

        public static double ReturnUnitPrice(Book book , int bookCount)
        {
            double discount = (100 - book.DiscountRate) / 100;
            if (bookCount < 5)
            {
                return book.Price * discount;
            }
            else if(bookCount < 10)
            {
                return book.Price5 * discount;
            }
            else if (bookCount < 10)
            {
                return book.Price10 * discount;
            }
            else
            {
                return book.Price20 * discount;
            }
        }
    }
}
