using Microsoft.AspNetCore.Mvc;
using Entity.EntityClass;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Repository.UnitOfWork;
using Repository.Interface;
using Entity.Models;
using Utility;
namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;
        private readonly IUnitOfWork unitofwork;
        private readonly IOrderRepository orderRepository;
        private readonly IGenericRepository<Order> _genericOrderRepository;
        public OrderController(UserManager<AppUser> _userManager, IMapper mapper, SignInManager<AppUser> signInManager, 
            INotyfService _notyf , IOrderRepository orderRepository , IUnitOfWork unitofwork)
        {
            this._mapper = mapper;
            this._notyf = _notyf;
            this.orderRepository = orderRepository;
            this.unitofwork = unitofwork;
            this._genericOrderRepository = unitofwork.GetRepository<Order>();
        }
        public IActionResult List(bool isApproved)
        {
            OrderViewPageModel model = new OrderViewPageModel(); // model for view 
            if (isApproved)
            {
                model.orderList = orderRepository.GetAllOrdersWithUsersAndDetails(x => x.OrderStatus == Entity.Enum.OrderStatus.Approved);
            }
            else
            {
                model.orderList = orderRepository.GetAllOrdersWithUsersAndDetails(x => x.OrderStatus == Entity.Enum.OrderStatus.Pending);
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult ApproveOrder(int orderid)
        {
            try
            {
                Order order = _genericOrderRepository.GetById(orderid);
                order.OrderStatus = Entity.Enum.OrderStatus.Approved;
                _notyf.Success(SD.OrderApproved);
            }
            catch(Exception ex)
            {
                _notyf.Success(SD.SomethingWentWrong);
            }
            return RedirectToAction("List" , "Order" , new {area = "Admin"});
        }

    }
}
