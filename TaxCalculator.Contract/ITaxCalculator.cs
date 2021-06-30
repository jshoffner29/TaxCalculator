namespace TaxCalculator.Contract
{
    /// <summary>
    /// Only supports Tax by Location
    /// Calculate the taxes for the order
    /// </summary>
    public interface ITaxCalculator
    {
        // Get the tax rate for a location
        decimal GetTaxRateForLocation(string zipcode);

        // Get taxes for the order
        void GetTaxForOrder();
    }
}
