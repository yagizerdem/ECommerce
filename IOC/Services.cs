using AutoMapper;
using DAL.dbcontext;
using Entity.AutoMapperProfile;
using Entity.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                options => options.UseSqlServer("Server=.;Database=bookecommerce;Trusted_Connection=True;TrustServerCertificate=True;", x => x.MigrationsAssembly("DAL")));
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ecommercedatabase>()
                .AddDefaultTokenProviders();

            // automapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

    }
}
