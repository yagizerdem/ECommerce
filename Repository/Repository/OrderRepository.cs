﻿using DAL.dbcontext;
using Entity.EntityClass;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        protected readonly ecommercedatabase _context;
        public OrderRepository(ecommercedatabase _context) : base(_context)
        {
            this._context = _context;
        }

        public List<Order> GetAllOrdersWithUsersAndDetails()
        {
           return _context.Orders.Include(x => x.OrderDetails).Include(x=>x.User).ToList();
        }
    }
}