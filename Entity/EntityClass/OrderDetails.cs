using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.EntityClass
{
    [Serializable]
    public class OrderDetails : BaseEntity
    {
        public int OrderId { get; set; }
        [Required]
        public Order Order { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ReciverFirstName { get; set; }
        public string ReciverLastName { get; set; }

        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
