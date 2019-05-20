// Demo 6 - Authentication Basics; LV
// Add this class to create a seperate model for the login credentials provided by the user

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace DroneWorks.Models
{
    public class LoginInput
    {
        [Required(ErrorMessage = "Please enter a username")]
        [MaxLength(50)]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z0-9\*\$]+$", ErrorMessage = "Letters, digits, *, $")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [UIHint("password")]
        [RegularExpression(@"^[a-zA-Z0-9\*\$]+$", ErrorMessage = "Letters, digits, *, $")]
        [MaxLength(50)]
        [MinLength(2)]
        public string Password { get; set; }

        public string ReturnURL { get; set; }
    }
}
