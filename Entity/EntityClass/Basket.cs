using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.EntityClass
{
    public class Basket :BaseEntity
    {
        public List<Card> Cards { get; set; }    
    
        public AppUser User { get; set; }   
        public int TotoalPrice { get; set; }

        public bool IsOrdered { get; set; }
    }
}
