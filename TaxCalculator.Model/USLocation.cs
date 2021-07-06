using System;

namespace TaxCalculator.Model
{
    public class USLocation : USState
    {
        public string Street { get; set; }
        public string City { get; set; }
        /// <summary>
        /// Only supports US 
        /// </summary>
        public string Country { get; } = "US";
        public string ZipCode { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public void ValidateRequiredFields()
        {
            if (string.IsNullOrWhiteSpace(Country) ||
                string.IsNullOrWhiteSpace(ZipCode) ||
                string.IsNullOrWhiteSpace(StateCode))
                throw new ArgumentNullException("The US Location zipcode and country are required fields.");
        }
    }
}
