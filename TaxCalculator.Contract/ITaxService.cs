using System.Collections.Generic;
using TaxCalculator.Model;

namespace TaxCalculator.Contract
{
    public interface ITaxService
    {
        /// <summary>
        /// Returns all order line item categories.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Category> GetCategories();
        /// <summary>
        /// Returns amount of sales tax to collect
        /// </summary>
        /// <param name="order">Order entity</param>
        /// <returns></returns>
        decimal GetTaxForOrder(Order order);
        /// <summary>
        /// Returns state sales tax rate for given location
        /// </summary>
        /// <param name="taxByLocation">From (optional) street and (required) zipcode entity.</param>
        /// <returns></returns>
        decimal GetTaxRateForLocation(TaxByLocation taxByLocation);
    }
}