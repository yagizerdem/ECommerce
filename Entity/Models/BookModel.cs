using Entity.EntityClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Entity.Models
{
    public class BookModel
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; }
        [Required]
        [MinLength(10)]
        public string SmallDescription { get; set; }
        [Required]
        [MinLength(30)]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double Price5 { get; set; }
        [Required]
        public double Price10 { get; set; }
        [Required]
        public double Price20 { get; set; }
        [Range(0, 100)]

        public double DiscountRate { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int StockCount { get; set; }
        [ValidateNever]
        public IFormFile HeaderImage { get; set; }
        [ValidateNever]
        public List<IFormFile> SubImages { get; set; }
    }
}
