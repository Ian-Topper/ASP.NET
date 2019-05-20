using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//add namespace

using System.ComponentModel.DataAnnotations;
using DroneWorks.Models;

namespace DroneWorks.Models
{
    public class CartItem
    {
        public Products Product { get; set; }

        [Required(ErrorMessage = "Please enter quantity")]
        [Range(2, 1000, ErrorMessage = "Please enter an amount between 1 and 20")]
        public int Quantity { get; set; }
    }
}
