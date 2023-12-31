﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.EntityClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Reflection.Emit;
using Microsoft.Extensions.Logging;
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

        public DbSet<Comment> Commnets { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

            // Configure the relationship
            builder.Entity<OrderDetails>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }
    }
}
