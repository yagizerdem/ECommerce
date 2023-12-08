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
        public List<Book> Books { get; set; } 

        public double TotalPrice { get; set; }
    }
}
