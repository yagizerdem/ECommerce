using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.EntityClass
{
    public class Order : BaseEntity
    {
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
        public string Address { get; set; }
        public string City { get; set; } 
        public bool IsPending { get; set; }
        public bool IsApproved { get; set; }
    }
}
