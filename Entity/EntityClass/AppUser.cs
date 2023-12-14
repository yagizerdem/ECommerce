using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.EntityClass
{
    public class AppUser : IdentityUser<string>
    {
        public AppUser()
    : base()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [Key]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfileImagePath { get; set; }
    }
}
