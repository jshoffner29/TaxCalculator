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
        void Initialize(string clientId);
        // Get the tax rate for a location
        decimal GetTaxRateForLocation(TaxByLocation taxByLocation);

        // Get taxes for the order
        decimal GetTaxForOrder(Order order);
        // Get all categories (from Tax Jar service)
        IEnumerable<Category> GetCategories();
    }
}
