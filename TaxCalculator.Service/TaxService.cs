using System.Collections.Generic;
using TaxCalculator.Contract;
using TaxCalculator.Model;

namespace TaxCalculator.Service
{
    public class TaxService : ITaxService
    {
        #region ctor
        private readonly ITaxCalculatorFactory taxCalculatorFactory;

        public TaxService(ITaxCalculatorFactory _taxCalculatorFactory)
        {
            taxCalculatorFactory = _taxCalculatorFactory;
        }
        #endregion

        public IEnumerable<Category> GetCategories()
        {
            return taxCalculatorFactory.GetCategories();
        }

        public decimal GetTaxForOrder(Order order)
        {
            return taxCalculatorFactory.GetTaxForOrder(order);
        }

        public decimal GetTaxRateForLocation(TaxByLocation taxByLocation)
        {
            return taxCalculatorFactory.GetTaxRateForLocation(taxByLocation);
        }

        #region Support Services
        public static List<USState> USStates
        {
            get
            {
                return new List<USState>
                    {
                    new USState("Alaska","AK"), new USState("Alabama", "AL"), new USState("Arkansas", "AR"),new USState("Arizona", "AZ"),
                    new USState("California", "CA"), new USState("Colorado", "CO"),new USState("Connecticut", "CT"), new USState("Delaware", "DE"),
                    new USState("Florida", "FL"),new USState("Georgia","GA"),new USState("Hawaii", "HI"),new USState("Iowa", "IA"),
                    new USState("Idaho", "ID"),new USState("Illinois", "IL"),new USState("Indiana", "IN"),new USState("Kansas", "KS"),
                    new USState("Kentucky", "KY"),new USState("Louisiana", "LA"),new USState("Massachusetts", "MA"), new USState("Maryland","MD"),
                    new USState("Maine", "ME"),new USState("Michigan", "MI"), new USState("Minnesota", "MN"),new USState("Missouri", "MO"),
                    new USState("Mississippi", "MS"), new USState("Montana", "MT"),new USState("North Carolina", "NC"), new USState("North Dakota", "ND"),
                    new USState("Nebraska", "NE"),new USState("New Hampshire","NH"), new USState("New Jersey", "NJ"), new USState("New Mexico", "NM"),
                    new USState("Nevada", "NV"), new USState("New York", "NY"), new USState("Ohio", "OH"),new USState("Oklahoma", "OK"),
                    new USState("Oregon", "OR"), new USState("Pennsylvania", "PA"),new USState("Rhode Island", "RI"),new USState("South Carolina","SC"),
                    new USState("South Dakota", "SD"),new USState("Tennessee", "TN"), new USState("Texas", "TX"),new USState("Utah", "UT"),
                    new USState("Virginia", "VA"),new USState("Vermont", "VT"),new USState("Washington", "WA"), new USState("Wisconsin", "WI"),
                    new USState("West Virginia","WV"),new USState("Wyoming", "WY") };
            }
        }
        #endregion
    }
}
