using System;
using System.Collections.Generic;

namespace DroneWorks.Models
{
    public partial class WorksOrder
    {
        public WorksOrder()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        public int OrderPk { get; set; }
        public int? UserFk { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipState { get; set; }
        public int? ShipZip { get; set; }
        public string ShipCountry { get; set; }
        public DateTime? ShipDate { get; set; }
        public string OrderStatus { get; set; }

        public virtual WorksUser UserFkNavigation { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
