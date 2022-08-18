using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace empcoreapiproj.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage ="Email is required")]
        public string? Email { get; set; }   
        [Required(ErrorMessage ="Password is required")]
        public string? Password { get; set; }
    }
}