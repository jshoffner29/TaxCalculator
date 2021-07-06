using System;
using System.Collections.Generic;
using System.Linq;
using TaxCalculator.Contract;
using TaxCalculator.Model;
using Taxjar;

namespace TaxCalculator.SupportService
{
    public abstract class TaxCalculatorTaxJarServiceBase: ITaxCalculatorService
    {
        public TaxjarApi Client { get; private set; }

        public TaxCalculatorTaxJarServiceBase()
        { }
        public void Initialize(string apikey)
        {
            Client = new TaxjarApi(apikey);
        }
        public decimal GetTaxForOrder(Model.Order order)
        {
            if (order == null)
                throw new ArgumentNullException("The order is null.");

            var taxForOrder = Client.TaxForOrder(order.GetOrder());

            if (taxForOrder == null)
                throw new ArgumentNullException("Unable to retrieve tax for order");

            return taxForOrder.AmountToCollect;
        }

        public decimal GetTaxRateForLocation(TaxByLocation taxByLocation)
        {
            if (taxByLocation == null)
                throw new ArgumentNullException("Entity object is null");

            if (string.IsNullOrWhiteSpace(taxByLocation.FromZipCode))
                throw new ArgumentNullException("The zip code value is required to find the tax rates for this location.");

            //var rates = client.RatesForLocation("90404-3370");
            RateResponseAttributes rates = null;
            try
            {
                rates = Client.RatesForLocation(taxByLocation.FromZipCode);
            }
            catch (Exception)
            {
                // swallow exception here. Error handling managed below.
            }            

            if (rates == null)
                throw new ArgumentNullException($"Unable to find the tax rates for zip code {taxByLocation.FromZipCode}");

            return rates.StateRate;
        }
        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Model.Category> GetCategories()
        {
            return Client.Categories()
                    .Select(s => new Model.Category
                    {
                        Name = s.Name,
                        ProductTaxCode = s.ProductTaxCode,
                        Description = s.Description
                    });
        }
    }
}
