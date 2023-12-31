﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Entity.EntityClass
{
    public class Book: BaseEntity
    {
        public string Title { get; set; }
        public string SmallDescription { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Price5 { get; set; }
        public double Price10 { get; set; }
        public double Price20 { get; set; }

        public List<Card> Cards { get; set; }
        public double DiscountRate { get; set; }
        public string Author { get; set; }

        public int StockCount { get; set; }
        public string? HeaderImagePath { get; set; }
        public List<Images> Images { get; set; }

    
    }
}
