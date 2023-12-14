using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class SingInViewModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "Longer First Name required")]
        [MaxLength(30, ErrorMessage = "First name exeed character limit")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Longer Last Name required")]
        [MaxLength(30, ErrorMessage = "Last name exeed character limit")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Stronger password required")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        [ValidateNever]
        public string UserRole { get; set; }
    }
}
