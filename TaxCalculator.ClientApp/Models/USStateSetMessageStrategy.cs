namespace TaxCalculator.ClientApp.Models
{
    public class USStateSetMessageStrategy : InstructionalMessageStrategy
    {
        public override void AlgorithmInterface()
        {
            SectionStateInstructions = "Now that a state has been selected, please select a zip code. Use the filtering" +
                " to find the specific city name or zip code being searched. Remember, you can always choose a different state.";
        }
    }
}
