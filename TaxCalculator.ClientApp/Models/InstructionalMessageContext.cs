namespace TaxCalculator.ClientApp.Models
{
    public class InstructionalMessageContext
    {
        public InstructionalMessageStrategy Strategy { get; set; }

        public InstructionalMessageContext(InstructionalMessageStrategy strategy)
        {
            Strategy = strategy;
            Update();
        }

        public void Update()
        {
            Strategy.AlgorithmInterface();
        }
    }
}