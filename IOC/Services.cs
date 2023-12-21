using AspNetCoreHero.ToastNotification;
using AutoMapper;
using DAL.dbcontext;
using Entity.AutoMapperProfile;
using Entity.EntityClass;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Repository.Interface;
using Repository.Repository;
using Repository.UnitOfWork;
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

            // sing in manager 
            services.AddScoped<SignInManager<AppUser>>();

            // automapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            // cookie authenticaiton
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Home/Index";
    });
            // toastr notificaiton 
            services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

            // unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // order repository 
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddHttpContextAccessor();

            services.AddSignalR();
        }
    }
}
