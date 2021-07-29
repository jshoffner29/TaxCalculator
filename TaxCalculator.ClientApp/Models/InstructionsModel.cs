namespace TaxCalculator.ClientApp.Models
{
    public class InstructionsModel
    {
        public string SectionAppInstructions { get; } = "Welcome to the Tax Service application! " +
            "This application let's the user view the tax rate for a given zip code and view the total tax amount for an order the user can create an order!";
        public string SectionStateInstructions { get; set; }
        public string SectionTaxForLocationInstructions { get; set; }
        public string SectionOrderInstructions { get; set; }
        public string SectionOrderTaxInstructions { get; set; }

        public InstructionsModel()
        {
            StateCodeCleared();
        }
        public void StateCodeCleared()
        {
            SectionStateInstructions = "Start by selecting a state (from the drop-down) and click the search button.";
        }
        public void StateCodeSelected()
        {
            SectionStateInstructions = "Now that a state has been selected, please select a zip code. Use the filtering" +
                    " to find the specific city name or zip code being searched." +
                " Remember, you can always choose a different state.";
        }
        public void ZipCodeSelected()
        {
            SectionTaxForLocationInstructions = "Click 'View Tax Rate' to see the tax rate for this zip code." +
                " Click 'View Zip Codes' to return to list of zip codes." +
                "Provide a street address for better accuracy.";

            SectionOrderTaxInstructions = "Hover over name and description to see full text.";

            NoOrderItems();
        }
        public void NoOrderItems()
        {
            SectionOrderInstructions = "Use the 'Add Order Item' form to create as many items as desired." +
                " Hover over name and description to see full text.";
        }
        public void OrderItemsSelected()
        {
            SectionOrderInstructions += "Use the 'Add Order Item' form to create as many items as desired." +
                " Hover over name and description to see full text. NOTE: Previously selected order items are" +
                " preserved even if the state and zip code are changed!";
        }
        public void TaxRateForLocationSelected(string zipCode, decimal taxRateForLocation)
        {
            SectionTaxForLocationInstructions = $"The tax rate for zip code {zipCode} is {taxRateForLocation:P}.";
        }
        public void OrderTaxAmountSelected(decimal orderTaxAmount)
        {
            SectionOrderTaxInstructions += $" The total tax for this order is {orderTaxAmount:C}.";
        }
    }
}