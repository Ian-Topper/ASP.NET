using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Threading.Tasks;


namespace DroneWorks.Models
{
    public partial class Products
    {
        public Products()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        public int ProdPk { get; set; }

       [Required(ErrorMessage = "Please enter a product name")]
       [MaxLength(50)]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z0-9\*\-\s\$]+$", ErrorMessage = "Letters, digits, -, *, $")]
        public string ProdName { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        [MaxLength(2000)]
        [MinLength(3)]
       [RegularExpression(@"^[a-zA-Z0-9\*\s\)\(\,\-\?\.\;\!\:\,\'\$]+$", ErrorMessage =
            "Please enter a description with these types Letters, digits, Sepcials: *,$?-;:'.!")]

        public string ProdDescribe { get; set; }


        [Required(ErrorMessage = "Please enter a product price")]
        [Range(maximum:99999, minimum:0, ErrorMessage = "Please enter a product price (Range 0-99999)")]
      //  [MinLength(1)]
        public decimal? ProdPrice { get; set; }
        [Required(ErrorMessage = "Please enter an image name or NA")]
        [MaxLength(50)]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z-0-9\s\.]+$", ErrorMessage = "Please enter an image name or NA")]

        public string ImageName { get; set; }
        public int? CatFk { get; set; }


        [Required(ErrorMessage = "Please enter total stock")]
        [Range(maximum: 99999, minimum: 0, ErrorMessage = "Please enter total stock (Range 0-99999)")]
        //  [MinLength(1)]
        public int? TotalStock { get; set; }

        public virtual Category CatFkNavigation { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
