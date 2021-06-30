using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Contract;
using TaxCalculator.SupportService;

namespace TaxCalculator.Service
{
    public class TaxService
    {
        private readonly ITaxCalculator taxCalculator;

        public TaxService(string clientId)
        {
            // TODO: clientId and associated apikey should be stored in config

            var apikey = "5da2f821eee4035db4771edab942a4cc";

            switch (clientId)
            {
                // Future client, which currently has no tax calculator implementation.
                case "ClientNoImplementation":

                    throw new ArgumentException($"Unable to find tax calculator for {clientId}", "clientid");

                    break;

                // Use Tax Jar calculator as default
                default:
                    // TODO: retrieve default config settings
                    // DEFAULT CLIENT: api key for default
                    taxCalculator = new TaxCalculatorTaxJar(apikey);
                    break;
            }

            // Consider a factory function that retrieves all of the support services
        }
        public void GetTaxForOrder()
        {
            taxCalculator.GetTaxForOrder();
        }

        public decimal GetTaxRateForLocation(string zipcode)
        {
            return taxCalculator.GetTaxRateForLocation(zipcode);
        }
    }
}
