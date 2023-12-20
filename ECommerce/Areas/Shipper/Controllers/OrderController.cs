using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Entity.EntityClass;
using Microsoft.AspNetCore.Mvc;
using NuGet.Frameworks;
using Repository.Interface;
using Repository.UnitOfWork;
using System.Security.Cryptography.X509Certificates;
using Utility;

namespace ECommerce.Areas.Shipper.Controllers
{
    [Area("Shipper")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitofwork;
        private readonly IGenericRepository<Order> orderRepository;
        private readonly INotyfService _notyf;
        private readonly IMapper _mapper;
        public OrderController(IUnitOfWork unitOfWork, INotyfService _notyf, IMapper _mapper, IWebHostEnvironment webhostenv)
        {
            this.unitofwork = unitOfWork;
            this.orderRepository = unitofwork.GetRepository<Order>();
            this._notyf = _notyf;
            this._mapper = _mapper;
        }
        public IActionResult ListApprovedOrders()
        {
            List<Order> orderList = orderRepository.Find(x => x.OrderStatus == Entity.Enum.OrderStatus.Approved , x=> x.User).ToList();
            return View(orderList);
        }

        public IActionResult ListOnTheWayOrders()
        {
            List<Order> orderList = orderRepository.Find(x => x.OrderStatus == Entity.Enum.OrderStatus.OnTheWay, x => x.User).ToList();
            return View(orderList);
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId , Entity.Enum.OrderStatus orderStatus)
        {
            try
            {
                Order order = orderRepository.GetById(orderId);
                order.OrderStatus = orderStatus;
                unitofwork.Commit();
                _notyf.Success(SD.OrderStausUpdated);
            }
            catch (Exception ex)
            {
                _notyf.Error(SD.SomethingWentWrong);
            }
            if(orderStatus == Entity.Enum.OrderStatus.OnTheWay) return RedirectToAction(nameof(ListApprovedOrders)); 
            else if(orderStatus == Entity.Enum.OrderStatus.Delivered) return RedirectToAction(nameof(ListOnTheWayOrders));
            return RedirectToAction(nameof(ListApprovedOrders));
        }


    }
}
