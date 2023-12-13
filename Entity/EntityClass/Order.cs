using Entity.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.EntityClass
{
    public class Order : BaseEntity
    {
        public List<OrderDetails> OrderDetails{ get; set; }
        public string UserId { get; set; }
        [Required]
        public AppUser User { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
