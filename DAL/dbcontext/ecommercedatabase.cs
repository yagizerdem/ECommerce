using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
namespace DAL.dbcontext
{
    public class ecommercedatabase : IdentityDbContext<AppUser , IdentityRole, string>
    {
        public ecommercedatabase(DbContextOptions<ecommercedatabase> options) : base(options)
        {
        }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Shipper> Shippers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique();
        }
    }
}
