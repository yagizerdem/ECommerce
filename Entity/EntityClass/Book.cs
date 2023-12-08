using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.EntityClass
{
    public class Book: BaseEntity
    {
        public string BookName { get; set; }

        public string Title { get; set; }

        public string SmallDescription { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Price5 { get; set; }
        public double Price10 { get; set; }
        public double Price20 { get; set; }

        public List<Card> Cards { get; set; }
        public double DiscountRate { get; set; }
    }
}
