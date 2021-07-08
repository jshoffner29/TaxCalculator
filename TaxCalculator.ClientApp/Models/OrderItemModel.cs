using TaxCalculator.Model;

namespace TaxCalculator.ClientApp.Models
{
    public class OrderItemModel : CategoryModel
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Hiding the CategoryModel's MapTo
        /// </summary>
        /// <returns></returns>
        public OrderLineItem MapTo()
        {
            return new OrderLineItem
            {
                Quanitity = Quantity,
                UnitPrice = UnitPrice
            };
        }
    }
}
