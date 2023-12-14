using AspNetCoreHero.ToastNotification.Extensions;
using ECommerce.SeedData;
using IOC;
using Stripe;

namespace ECommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // register services from IOC tier
            IOC.Services.Register(builder.Services);
            // stripe webhook service

            var app = builder.Build();

            DbInitializer.Initialize(app); // seed data

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            builder.Services.AddOptions();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["SecretKey"];
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "Admin",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseNotyf();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run();

        }
    }
}
