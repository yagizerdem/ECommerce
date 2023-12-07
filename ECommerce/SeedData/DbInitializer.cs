using DAL.dbcontext;
using Entity.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.SeedData
{
    public static class DbInitializer
    {
        public static async Task Initialize(IApplicationBuilder app)
        {
            const string AdminUserName = "admin";
            const string AdminPassword = "SifreBu123!";
            const string AdminEmail = "admin@gmail.com";
            string[] Roles = new string[3] { "Admin", "Author", "Customer" };

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ecommercedatabase>();

                var _userManager =
                         serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
                var _roleManager =
                         serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();


                try
                {
                    foreach (var role in Roles)
                    {
                        if (!_roleManager.RoleExistsAsync(role).Result)
                        {
                            await _roleManager.CreateAsync(new IdentityRole { Name = role });
                        }
                    }
                }catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                var adminUser = await _userManager.FindByEmailAsync(AdminEmail);
                if (adminUser == null)
                {
                    try
                    {
                        AppUser adminuser = new AppUser()
                        {
                            Id = new Guid().ToString(),
                            FirstName = AdminUserName,
                            LastName = AdminUserName,
                            UserName = AdminUserName,
                            EmailConfirmed = true,
                            Email = AdminEmail,
                        };
                        await _userManager.CreateAsync(adminuser, AdminPassword);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                await   context.SaveChangesAsync();
            }
        }
    }
}
