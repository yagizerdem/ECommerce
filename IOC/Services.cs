using DAL.dbcontext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC
{
    public class Services
    {
        public static void Register(IServiceCollection services)
        {
            services.AddDbContext<ecommercedatabase>(
             options => options.UseSqlServer("Server=.;Database=bookecommerce;Trusted_Connection=True;TrustServerCertificate=True;",x=> x.MigrationsAssembly("DAL")));
        }

    }
}
