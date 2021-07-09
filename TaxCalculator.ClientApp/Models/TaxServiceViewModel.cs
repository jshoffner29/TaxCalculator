using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaxCalculator.ClientApp.Models
{
    public class TaxServiceViewModel
    {
        public string StateCodeSelected { get; set; }
        public string ZipCodeSelected { get; set; }
        public string StreetSelected { get; set; }
        public OrderItemModel OrderItemSelected { get; set; }
        public decimal TaxRateForLocation { get; set; }
        public bool TaxRateForLocationWasSet { get; set; }
        public static List<USStateModel> USStates { get; set; }
        public List<USLocationModel> USSlocations { get; set; }
        /// <summary>
        /// Indexed collection (index is product tax code)
        /// </summary>
        public static Dictionary<string,CategoryModel> Categories { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
        public decimal OrderTaxAmount { get; set; }
        public string SectionStateInstructions { get; set; }
        public string SectionTaxForLocationInstructions{ get; set; }
        public string SectionOrderInstructions { get; set; }
        public string SectionOrderTaxInstructions { get; set; }
        public TaxServiceViewModel()
        {
            OrderItems = new List<OrderItemModel>();
        }

        internal void AddOrderItem()
        {
            if (OrderItemSelected == null)
                throw new ArgumentNullException("OrderItemSelected", "No order item found.");

            var activeCategory = Categories[OrderItemSelected.ProductTaxCode];
            OrderItemSelected.Name = activeCategory.Name;
            OrderItemSelected.Description = activeCategory.Description;

            OrderItems.Add(OrderItemSelected);
        }
        internal void DeleteOrderItem(int index)
        {
            OrderItems.RemoveAt(index);
        }
        internal OrderModel GetOrder()
        {
            // use the same location for both 'TO' and 'FROM' locations
            var usLocation = new USLocationModel { StateCode = StateCodeSelected, ZipCode = ZipCodeSelected };

            return new OrderModel(usLocation, usLocation, OrderItems);
        }

        #region Support Methods
        public string GetShortText(string text, int length = 10)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            return text.Substring(0, Math.Min(length, text.Length)) + "...";
        }
        public IEnumerable<SelectListItem> GetViewList<T>()
        {
            IEnumerable<SelectListItem> viewCollection = null;

            if (typeof(T) == typeof(USStateModel))
            {
                viewCollection = (USStates as IEnumerable<USStateModel>).Select(s => new SelectListItem
                {
                    Text = s.StateName,
                    Value = s.StateCode
                });
            } else if(typeof(T) == typeof(CategoryModel))
            {
                viewCollection = (Categories.Values as IEnumerable<CategoryModel>).Select(s => new SelectListItem
                {
                    // Return a max length of 10 characters for category name.
                    Text = s.ProductTaxCode + "-" + s.Name.Substring(0,Math.Min(s.Name.Length,10)) + (s.Name.Length > 10 ? "...": ""),
                    Value = s.ProductTaxCode
                });
            }

            if (viewCollection == null)
                throw new ArgumentNullException("The view collection was not instantiated.");

            return viewCollection;
        }
        #endregion
    }
}
