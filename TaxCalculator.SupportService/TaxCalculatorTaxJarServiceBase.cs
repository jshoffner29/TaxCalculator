using System;
using System.Collections.Generic;
using System.Linq;
using TaxCalculator.Contract;
using TaxCalculator.Model;
using Taxjar;

namespace TaxCalculator.SupportService
{
    public abstract class TaxCalculatorTaxJarServiceBase : ITaxCalculatorService
    {
        private TaxjarApi client;

        public TaxCalculatorTaxJarServiceBase()
        { }
        public decimal GetTaxForOrder(Model.Order order)
        {
            if (order == null)
                throw new ArgumentNullException("The order is null.");

            var taxForOrder = client.TaxForOrder(order.GetOrder());

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

            RateResponseAttributes rates = null;
            try
            {
                rates = client.RatesForLocation(taxByLocation.FromZipCode);
            }
            catch (Exception)
            {
                // swallow exception here. Error handling managed below.
            }

            if (rates == null)
                throw new ArgumentNullException($"Unable to find the tax rates for zip code {taxByLocation.FromZipCode}");

            return rates.StateRate;
        }
        public IEnumerable<Model.Category> GetCategories()
        {
            return client.Categories()
                    .Select(s => new Model.Category
                    {
                        Name = s.Name,
                        ProductTaxCode = s.ProductTaxCode,
                        Description = s.Description
                    });
        }

        #region Support Methods
        public virtual void Initialize(string apikey)
        {
            client = new TaxjarApi(apikey);
        }
        #endregion
    }
}
