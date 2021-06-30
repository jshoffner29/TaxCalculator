using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Contract;
using Taxjar;

namespace TaxCalculator.SupportService
{
    public class TaxCalculatorTaxJar : ITaxCalculator
    {
        private readonly TaxjarApi client;

        public TaxCalculatorTaxJar()
        {
            var apikey = "5da2f821eee4035db4771edab942a4cc";
            client = new TaxjarApi(apikey);
        }

        public void GetTaxForOrder()
        {
            throw new NotImplementedException();
        }

        public decimal GetTaxRateForLocation(string zipcode)
        {
            //var rates = client.RatesForLocation("90404-3370");
            var rates = client.RatesForLocation(zipcode);

            return rates.StateRate;
        }
    }
}
