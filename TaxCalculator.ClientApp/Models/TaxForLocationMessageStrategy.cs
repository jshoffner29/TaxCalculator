namespace TaxCalculator.ClientApp.Models
{
    public class TaxForLocationMessageStrategy : InstructionalMessageStrategy
    {
        public override void AlgorithmInterface()
        {
            SectionStateInstructions = "Now that a state has been selected, please select a zip code. Use the filtering" +
                " to find the specific city name or zip code being searched. Remember, you can always choose a different state.";

            SectionTaxForLocationInstructions = $"The tax rate for zip code {ZipCode} is {TaxRateForLocation:P}.";

            SectionOrderInstructions += " NOTE: Previously selected order items are preserved" +
                        " even if the state and zip code are changed!";
        }
    }
}
