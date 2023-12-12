using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ECommerce.Controllers
{
    public class PurchaseController : Controller
    {
        [HttpPost]
        public JsonResult AddToBasket([FromBody] PurchaseRequestModel purchaseRequest)
        {
            
            return Json(new { result = "success" });
        }
    }
}
