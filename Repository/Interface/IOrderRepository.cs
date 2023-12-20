using Entity.EntityClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IOrderRepository
    {
        public List<Entity.EntityClass.Order> GetAllOrdersWithUsersAndDetails(Expression<Func<Order, bool>> expression);
    }
}
