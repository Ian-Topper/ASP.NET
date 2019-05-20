using System;
using System.Collections.Generic;

namespace DroneWorks.Models
{
    public partial class OrderItem
    {
        public int OrderItemPk { get; set; }
        public int? OrderFk { get; set; }
        public int? ProdFk { get; set; }
        public int? Quantity { get; set; }

        public virtual WorksOrder OrderFkNavigation { get; set; }
        public virtual Products ProdFkNavigation { get; set; }
    }
}
