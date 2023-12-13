using Entity.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.EntityClass
{
    public class Basket :BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public List<Card> Cards { get; set; }    
        public double TotoalPrice { get; set; }

        public BasketStatus status { get; set; }
    }
}
