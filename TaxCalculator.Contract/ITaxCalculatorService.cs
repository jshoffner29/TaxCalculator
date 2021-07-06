using System.Collections.Generic;
using TaxCalculator.Model;
namespace TaxCalculator.Contract
{
    /// <summary>
    /// Only supports Tax by Location
    /// Calculate the taxes for the order
    /// </summary>
    public interface ITaxCalculatorService
    {
        /// <summary>
        /// Initialize the service.
        /// </summary>
        /// <param name="clientId">Application's client</param>
        void Initialize(string clientId);
        /// <summary>
        /// Get state sales tax rate for given location
        /// </summary>
        /// <param name="taxByLocation">From (optional) street and (required) zipcode entity.</param>
        /// <returns></returns>
        decimal GetTaxRateForLocation(TaxByLocation taxByLocation);

        /// <summary>
        /// Get amount of sales tax to collect
        /// </summary>
        /// <param name="order">Order entity</param>
        /// <returns></returns>
        decimal GetTaxForOrder(Order order);
        /// <summary>
        /// Get all order line item categories.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Category> GetCategories();
    }
}
