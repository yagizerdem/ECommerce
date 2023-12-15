using DAL.dbcontext;
using Entity.EntityClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CustomerAPIController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        //private readonly ecommercedatabase _ecommercedatabase;
        public CustomerAPIController(UserManager<AppUser> _userManager , ecommercedatabase _ecommercedatabase)
        {
            this._userManager = _userManager;
            //this._ecommercedatabase = _ecommercedatabase;
        }
        [HttpPost("GetCustomers")]
        public IActionResult GetCustomers()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                var users = _userManager.Users.AsQueryable();

                // Apply sorting if specified
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    users = users.OrderBy(sortColumn + " " + sortColumnDirection);
                //}

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    users = users.Where(m => m.FirstName.Contains(searchValue)
                                        || m.LastName.Contains(searchValue)
                                        || m.Email.Contains(searchValue));
                }

                var recordsTotal = users.Count();

                // Apply pagination
                var data = users
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(user => new
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        UserName = user.UserName,
                        // other properties...                     
                    })
                    .ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("TestApi")]
        public IActionResult TestApi()
        {
            var data = new
            {
                Property1 = "Value1",
                Property2 = "Value2",
                // Add more properties as needed
            };
            return Ok("api is working");
        }
    }
}
