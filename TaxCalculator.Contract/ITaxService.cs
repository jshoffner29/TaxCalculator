using System.Collections.Generic;
using TaxCalculator.Model;

namespace TaxCalculator.Contract
{
    public interface ITaxService
    {
        IEnumerable<Category> GetCategories();
        decimal GetTaxForOrder(Order order);
        decimal GetTaxRateForLocation(TaxByLocation taxByLocation);
        IEnumerable<USLocation> GetUSLocations(string stateCode);
    }
}