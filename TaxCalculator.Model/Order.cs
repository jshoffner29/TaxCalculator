using System;
using System.Collections.Generic;
using System.Linq;

namespace TaxCalculator.Model
{
    public class Order : TaxByLocation
    {
        public USLocation USLocationFrom { get; set; }
        public USLocation USLocationTo { get; set; }
        public int Shipping { get; } = 0;
        public List<OrderLineItem> LineItems { get; set; }

        public Order()
        {
            LineItems = new List<OrderLineItem>();
        }
        public void Validate()
        {
            if (USLocationFrom == null || USLocationTo == null)
                throw new ArgumentNullException("Both the US Locations TO and FROM information is required.");

            // Validate required fields
            try
            {
                USLocationFrom.ValidateRequiredFields();
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("The US Location FROM street, city, state code, zipcode, and country are required fields.");
            }
            try
            {
                USLocationTo.ValidateRequiredFields();
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("The US Location TO zipcode and country are required fields.");
            }

            if (!LineItems.Any())
                throw new ArgumentNullException("Must have at least one line item.");
        }
        public object GetOrder()
        {
            Validate();
            return new
            {
                from_country = USLocationFrom.Country,
                from_zip = USLocationFrom.ZipCode,
                from_state = USLocationFrom.StateCode,
                from_city = USLocationFrom.City,
                from_street = USLocationFrom.Street,
                to_country = USLocationTo.Country,
                to_zip = USLocationTo.ZipCode,
                to_state = USLocationTo.StateCode,
                to_city = USLocationTo.City,
                to_street = USLocationTo.Street,
                shipping = Shipping,
                line_items = GetLineItems()
            };
        }
        #region Support Method
        private object GetLineItems()
        {
            var results = new List<object>();

            foreach (var item in LineItems)
            {
                results.Add(
                    new
                    {
                        quantity = item.Quanitity,
                        unit_price = item.UnitPrice
                    });
            }

            return results.ToArray();
        }
        #endregion
        }
}
