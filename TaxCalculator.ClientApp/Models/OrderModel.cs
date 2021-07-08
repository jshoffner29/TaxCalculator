using System.Collections.Generic;
using System.Linq;
using TaxCalculator.Model;

namespace TaxCalculator.ClientApp.Models
{
    public class OrderModel
    {
        public USLocationModel USLocationFrom { get; set; }
        public USLocationModel USLocationTo { get; set; }
        public int Shipping { get; } = 0;
        public List<OrderItemModel> LineItems { get; set; }

        public OrderModel(USLocationModel from, USLocationModel to, List<OrderItemModel> lineItems)
        {
            USLocationFrom = from;
            USLocationTo = to;
            LineItems = lineItems;
        }

        public Order MapTo()
        {
            return new Order
            {
                USLocationFrom = USLocationFrom.MapTo(),
                USLocationTo = USLocationTo.MapTo(),
                LineItems = LineItems.Select(s => s.MapTo()).ToList()
            };
        }
    }
}
