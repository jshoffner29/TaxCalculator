using System;
using System.Collections.Generic;
using System.Linq;
using TaxCalculator.Model;
using Taxjar;

namespace TaxCalculator.SupportService
{
    public class TaxJarFactory : TaxCalculatorFactoryBase
    {
        #region ctor
        private readonly TaxjarApi client;
        private const string apikey = "5da2f821eee4035db4771edab942a4cc";
        public TaxJarFactory()
        {
            client = new TaxjarApi(apikey);
        }
        #endregion

        public override decimal GetTaxRateForLocation(TaxByLocation taxByLocation)
        {
            if (taxByLocation == null)
                throw new ArgumentNullException("Entity object is null");

            if (string.IsNullOrWhiteSpace(taxByLocation.FromZipCode))
                throw new ArgumentNullException("The zip code value is required to find the tax rates for this location.");

            RateResponseAttributes rates = null;
            try
            {
                rates = string.IsNullOrWhiteSpace(taxByLocation.FromStreet)
                    ? client.RatesForLocation(taxByLocation.FromZipCode)
                    : client.RatesForLocation(taxByLocation.FromZipCode, new
                    {
                        street = taxByLocation.FromStreet,
                        country = "US"
                    });
            }
            catch (Exception)
            {
                // swallow exception here. Error handling managed below.
            }

            if (rates == null)
                throw new ArgumentNullException($"Unable to find the tax rates for zip code {taxByLocation.FromZipCode}");

            return rates.StateRate;
        }

        public override decimal GetTaxForOrder(Model.Order order)
        {
            if (order == null)
                throw new ArgumentNullException("The order is null.");

            var taxForOrder = client.TaxForOrder(order.GetOrder());

            if (taxForOrder == null)
                throw new ArgumentNullException("Unable to retrieve tax for order");

            return taxForOrder.AmountToCollect;
        }

        /// <summary>
        /// Get all order line item categories.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Model.Category> GetCategories()
        {
            return client.Categories()
                    .Select(s => new Model.Category
                    {
                        Name = s.Name,
                        ProductTaxCode = s.ProductTaxCode,
                        Description = s.Description
                    });
        }
    }
}