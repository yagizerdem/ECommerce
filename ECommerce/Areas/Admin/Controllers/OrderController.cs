using Microsoft.AspNetCore.Mvc;
using Entity.EntityClass;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Repository.UnitOfWork;
using Repository.Interface;
using Entity.Models;
namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;
        private readonly IUnitOfWork unitofwork;
        private readonly IOrderRepository orderRepository;
        public OrderController(UserManager<AppUser> _userManager, IMapper mapper, SignInManager<AppUser> signInManager, 
            INotyfService _notyf , IOrderRepository orderRepository)
        {
            this._mapper = mapper;
            this._notyf = _notyf;
            this.orderRepository = orderRepository;
        }
        public IActionResult List()
        {
            OrderViewPageModel model = new OrderViewPageModel(); // model for view 
            model.orderList = orderRepository.GetAllOrdersWithUsersAndDetails();
            return View(model);
        }
    }
}
