using System.ComponentModel.DataAnnotations;
using TaxCalculator.Model;

namespace TaxCalculator.ClientApp.Models
{
    public class OrderItemModel : CategoryModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a quantity value greater than 0.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a value greater than 0.")]
        public int? Quantity { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a price greater than 0.")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid currency price.")]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Please provide a price greater than 0.")]
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Hiding the CategoryModel's MapTo
        /// </summary>
        /// <returns></returns>
        public OrderLineItem MapTo()
        {
            return new OrderLineItem
            {
                Quanitity = Quantity.GetValueOrDefault(),
                UnitPrice = UnitPrice.GetValueOrDefault()
            };
        }
    }
}
