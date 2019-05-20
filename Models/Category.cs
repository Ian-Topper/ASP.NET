using System;
using System.Collections.Generic;

namespace DroneWorks.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Products>();
        }

        public int CatPk { get; set; }
        public string Type { get; set; }
        public string CatDescribe { get; set; }
        public string CatImage { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
