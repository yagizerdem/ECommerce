using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace DAL.dbcontext
{
    public class ecommercedatabase : IdentityDbContext<AppUser , IdentityRole, string>
    {
        public ecommercedatabase(DbContextOptions<ecommercedatabase> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


    }
}
