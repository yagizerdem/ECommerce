using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Repository.UnitOfWork;
using Utility;
using Entity.EntityClass;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class ShipperController : Controller
    {
        private readonly IUnitOfWork unitofwork;
        private readonly IGenericRepository<Entity.EntityClass.Shipper> shipperRepository;
        private readonly INotyfService _notyf;
        private readonly IMapper _mapper;

        public ShipperController(IUnitOfWork unitOfWork, INotyfService _notyf, IMapper _mapper)
        {
            unitofwork = unitOfWork;
            shipperRepository = unitofwork.GetRepository<Entity.EntityClass.Shipper>();
            this._notyf = _notyf;
            this._mapper = _mapper;
        }
        public IActionResult List()
        {
            List<Entity.EntityClass.Shipper> list = (List<Entity.EntityClass.Shipper>)shipperRepository.GetAll();
            return View(list);
        }

        public IActionResult AddShipper()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddShipper(string shippername)
        {
            try
            {
                var isThereShipperName = shipperRepository.Find(x => x.ShipperName == shippername).FirstOrDefault();
                if (isThereShipperName != null)
                {
                    _notyf.Error(SD.ThereIsAlredyShipper);
                    return RedirectToAction("List", "Shipper", new { area = "Admin" });
                }
                // go to db here
                Entity.EntityClass.Shipper shipper = new Entity.EntityClass.Shipper()
                {
                    ShipperName = shippername,
                    ShipperCode = RandomStringGenerator.GenerateMini(),
                };
                shipperRepository.Add(shipper);
                unitofwork.Commit();
            }
            catch (Exception ex)
            {
                _notyf.Error(SD.ShipperAddedFail);
            }
            _notyf.Success(SD.ShipperAddedSuccessfull);
            return RedirectToAction("List" , "Shipper" , new { area = "Admin"} );
        }

        [HttpPost]
        public IActionResult Delete(int shipperid)
        {
            try
            {
                Entity.EntityClass.Shipper shipperformdb = shipperRepository.GetById(shipperid);
                shipperRepository.Remove(shipperformdb); // delte enityt form db compeletely
                unitofwork.Commit();
            }
            catch (Exception ex)
            {
                _notyf.Error(SD.SomethingWentWrong);
                return RedirectToAction("List", "Shipper", new { area = "Admin" });
            }

            _notyf.Success(SD.ShipperDeletedSuccess);
            return RedirectToAction("List", "Shipper", new { area = "Admin" });
        }
        
        public IActionResult Update(int shipperid)
        {
            Entity.EntityClass.Shipper shipper = shipperRepository.GetById(shipperid);
            TempData["shipperid"] = shipper.Id;
            return View(shipper);
        }

        [HttpPost]
        public IActionResult Update(string shippername)
        {
            int shipperid = (int)TempData["shipperid"];
            try
            {
                Entity.EntityClass.Shipper shipperformdb = shipperRepository.GetById(shipperid);
                shipperformdb.ShipperName = shippername;
                shipperRepository.Update(shipperformdb);
                unitofwork.Commit();
            }
            catch (Exception ex)
            {
                _notyf.Error(SD.SomethingWentWrong);
                return RedirectToAction("List", "Shipper", new { area = "Admin" });
            }
            _notyf.Success(SD.ShipperUpdatedSuccess);
            return RedirectToAction("List", "Shipper", new { area = "Admin" });
        }
    }
}
