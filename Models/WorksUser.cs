using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace DroneWorks.Models
{
    public partial class WorksUser
    {
        public WorksUser()
        {
            WorksOrder = new HashSet<WorksOrder>();
        }
        public int UserPk { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        [MaxLength(50)]
        [MinLength(2)]
        //[RegularExpression(@"^[a-zA-Z0-9\*\$]+$", ErrorMessage = "Letters, digits, *, $")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [MaxLength(50)]
        [MinLength(2)]
        //[RegularExpression(@"^[a-zA-Z0-9\*\$]+$", ErrorMessage = "Letters, digits, *, $")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your address")]
        [MaxLength(50)]
        [MinLength(2)]
        //[RegularExpression(@"^[a-zA-Z0-9\*\$]+$", ErrorMessage = "Letters, digits, *, $")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter City")]
        [MaxLength(50)]
        [MinLength(2)]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Letters only")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter State")]
        [MaxLength(50)]
        [MinLength(2)]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Letters only")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please enter Zipcode")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numbers only")]
        public int? Zip { get; set; }

        [Required(ErrorMessage = "Please enter Country")]
        [MaxLength(50)]
        [MinLength(2)]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Letters only")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [MaxLength(50)]
        [MinLength(2)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Please enter Country")]
        [MaxLength(10)]
        [MinLength(8)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter Login")]
        [MaxLength(50)]
        [MinLength(5)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [MaxLength(50)]
        [MinLength(2)]
        public string Password { get; set; }

        public int? RoleFk { get; set; }

        public virtual UserRole RoleFkNavigation { get; set; }
        public virtual ICollection<WorksOrder> WorksOrder { get; set; }
    }
}
