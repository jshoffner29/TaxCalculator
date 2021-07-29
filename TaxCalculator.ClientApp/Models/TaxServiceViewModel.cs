using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using TaxCalculator.Contract;
using TaxCalculator.Model;

namespace TaxCalculator.ClientApp.Models
{
    public class TaxServiceViewModel
    {
        public string StateCodeSelected { get; set; }
        public string ZipCodeSelected { get; set; }
        public string StreetSelected { get; set; }        
        public decimal TaxRateForLocation { get; set; }
        public bool TaxRateForLocationWasSet { get; set; }

        public static List<USStateModel> USStates { get; set; }
        public List<USLocationModel> USSlocations { get; set; }
        public string FilteredCityNameSelected { get; set; }
        public string FilteredZipCodeSelected { get; set; }
        public List<USLocationModel> FilteredUSSLocations { get; set; }
        /// <summary>
        /// Indexed collection (index is product tax code)
        /// </summary>
        public static Dictionary<string, CategoryModel> Categories { get; set; }
        public OrderItemModel OrderItemSelected { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
        public decimal OrderTaxAmount { get; set; }

        #region Instructions
        //public InstructionsModel InstructionsModel { get; private set; }
        public string SectionAppInstructions { get; } = "Welcome to the Tax Service application! " +
            "This application let's the user view the tax rate for a given zip code and view the total tax amount for an order the user can create an order!";
        public string SectionStateInstructions { get; set; }
        public string SectionTaxForLocationInstructions { get; set; }
        public string SectionOrderInstructions { get; set; }
        public string SectionOrderTaxInstructions { get; set; }
        #endregion
        public TaxServiceViewModel()
        {
            OrderItems = new List<OrderItemModel>();
        }
        internal void SetStateCode(IZipCodeService zipCodeService, string stateCode)
        {
            StateCodeSelected = stateCode;
            ZipCodeSelected = string.Empty;
            USSlocations = zipCodeService
                                        .GetUSLocations(StateCodeSelected)
                                        .Select(s => new USLocationModel(s))
                                        .ToList();

            FilteredUSSLocations = new List<USLocationModel>(USSlocations);

            TaxRateForLocation = -1;
            OrderTaxAmount = -1;           
        }
        internal void SetFilteredUSLocations(string cityName, string zipCode)
        {
            FilteredUSSLocations = new List<USLocationModel>(USSlocations);

            if (string.IsNullOrEmpty(cityName) && string.IsNullOrEmpty(zipCode))
            {
                FilteredCityNameSelected = string.Empty;
                FilteredZipCodeSelected = string.Empty;
                return;
            }

            var filterOnZipCode = !Equals(zipCode, FilteredZipCodeSelected);

            FilteredUSSLocations = filterOnZipCode
                ? FilteredUSSLocations.Where(s => s.ZipCode.Equals(zipCode)).ToList()
                : FilteredUSSLocations.Where(s => s.City.Equals(cityName)).ToList();

            if(filterOnZipCode)
            {
                FilteredZipCodeSelected = zipCode;
                FilteredCityNameSelected = string.Empty;
            } else
            {
                FilteredCityNameSelected = cityName;
                FilteredZipCodeSelected = string.Empty;
            }
            
        }
        internal void ClearFilteredUSLocations()
        {
            FilteredCityNameSelected = string.Empty;
            FilteredZipCodeSelected = string.Empty;
        }
        internal void SetZipCode(string zipCode)
        {
            ZipCodeSelected = zipCode;
            TaxRateForLocation = -1;
        }
        internal void SetTaxRateForLocation(ITaxService taxService, string street)
        {
            if (string.IsNullOrEmpty(ZipCodeSelected)) return; // do nothing

            StreetSelected = street;
            var taxByLocation = new TaxByLocation { FromStreet = StreetSelected, FromZipCode = ZipCodeSelected };
            TaxRateForLocation = taxService.GetTaxRateForLocation(taxByLocation);
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
        private OrderModel GetOrder()
        {
            // use the same location for both 'TO' and 'FROM' locations
            var usLocation = new USLocationModel { StateCode = StateCodeSelected, ZipCode = ZipCodeSelected };

            if (!string.IsNullOrEmpty(StreetSelected))
            {
                usLocation.Street = StreetSelected;
            }

            return new OrderModel(usLocation, usLocation, OrderItems);
        }
        internal void SetInstructions()
        {
            if (string.IsNullOrEmpty(StateCodeSelected))
            {
                SectionStateInstructions = "Start by selecting a state (from the drop-down) and click the search button.";
            }
            else
            {
                SectionStateInstructions = "Now that a state has been selected, please select a zip code. Use the filtering" +
                    " to find the specific city name or zip code being searched." +
                " Remember, you can always choose a different state.";
            }

            if (!string.IsNullOrEmpty(ZipCodeSelected))
            {
                SectionTaxForLocationInstructions = "Click 'View Tax Rate' to see the tax rate for this zip code." +
                " Click 'View Zip Codes' to return to list of zip codes." +
                "Provide a street address for better accuracy.";

                SectionOrderInstructions = "Use the 'Add Order Item' form to create as many items as desired." +
                    " Hover over name and description to see full text.";

                SectionOrderTaxInstructions = "Hover over name and description to see full text.";

                if (OrderItems.Any())
                {
                    SectionOrderInstructions += " NOTE: Previously selected order items are preserved" +
                        " even if the state and zip code are changed!";
                }
            }

            if(TaxRateForLocation != -1)
            {
                SectionTaxForLocationInstructions = $"The tax rate for zip code {ZipCodeSelected} is {TaxRateForLocation:P}.";
            }
            if(OrderTaxAmount != -1)
            {
                SectionOrderTaxInstructions += $" The total tax for this order is {OrderTaxAmount:C}.";
            }
        }
        internal void SetModel(ITaxService taxService)
        {
            SetInstructions();
            if (OrderItems.Any() && !string.IsNullOrEmpty(ZipCodeSelected))
            {
                var order = GetOrder().MapTo();
                OrderTaxAmount = taxService.GetTaxForOrder(order);
            }
            else
            {
                OrderTaxAmount = -1; // indicate no tax calculated
            }
        }
        #region Support Methods
        public string GetShortText(string text, int length = 10)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            return text.Substring(0, Math.Min(length, text.Length)) + "...";
        }
        public IEnumerable<SelectListItem> GetViewList<T>(string fieldName = null)
        {
            IEnumerable<SelectListItem> viewCollection = null;

            if (typeof(T) == typeof(USStateModel))
            {
                viewCollection = (USStates as IEnumerable<USStateModel>).Select(s => new SelectListItem
                {
                    Text = s.StateName,
                    Value = s.StateCode
                });
            }
            else if (typeof(T) == typeof(CategoryModel))
            {
                viewCollection = (Categories.Values as IEnumerable<CategoryModel>).Select(s => new SelectListItem
                {
                    // Return a max length of 10 characters for category name.
                    Text = s.ProductTaxCode + "-" + s.Name.Substring(0, Math.Min(s.Name.Length, 10)) + (s.Name.Length > 10 ? "..." : ""),
                    Value = s.ProductTaxCode
                });
            }
            else if (typeof(T) == typeof(USLocationModel))
            {
                viewCollection = Equals(nameof(FilteredCityNameSelected),fieldName)
                    ? (USSlocations as IEnumerable<USLocationModel>).Select(s => new SelectListItem
                        {
                            Text = s.City,
                            Value = s.City
                        }).OrderBy(o => o.Text)
                    : (USSlocations as IEnumerable<USLocationModel>).Select(s => new SelectListItem
                    {
                        Text = s.ZipCode,
                        Value = s.ZipCode
                    });
            }

            if (viewCollection == null)
                throw new ArgumentNullException("The view collection was not instantiated.");

            return viewCollection;
        }
        #endregion
    }
}
