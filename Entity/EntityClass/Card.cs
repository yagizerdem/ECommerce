using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Entity.EntityClass
{
    public class Card : BaseEntity
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int BasketId { get; set; }
        public Basket Basket { get; set; }
        public double TotalPrice { get; set; }

        public int BookCount { get; set; }  

    }
}
