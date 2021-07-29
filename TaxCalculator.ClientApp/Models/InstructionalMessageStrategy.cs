namespace TaxCalculator.ClientApp.Models
{
    public abstract class InstructionalMessageStrategy
    {
        public string SectionAppInstructions { get; } = "Welcome to the Tax Service application! " +
            "This application let's the user view the tax rate for a given zip code and view the total tax amount for an order the user can create an order!";
        public string SectionStateInstructions { get; set; }
        public string SectionTaxForLocationInstructions { get; set; }
        public string SectionOrderInstructions { get; set; }
        public string SectionOrderTaxInstructions { get; set; }

        public string ZipCode { get; set; }
        public decimal TaxRateForLocation { get; set; }

        public abstract void AlgorithmInterface();
    }
}
