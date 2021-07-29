namespace TaxCalculator.ClientApp.Models
{
    public class ZipCodeSetMessageStrategy : InstructionalMessageStrategy
    {
        public override void AlgorithmInterface()
        {
            SectionStateInstructions = "Now that a state has been selected, please select a zip code. Use the filtering" +
                " to find the specific city name or zip code being searched. Remember, you can always choose a different state.";

            SectionTaxForLocationInstructions = "Click 'View Tax Rate' to see the tax rate for this zip code." +
                " Click 'View Zip Codes' to return to list of zip codes." +
                "Provide a street address for better accuracy.";

            SectionOrderInstructions = "Use the 'Add Order Item' form to create as many items as desired." +
                " Hover over name and description to see full text.";

            SectionOrderTaxInstructions = "Hover over name and description to see full text.";
        }
    }
}
