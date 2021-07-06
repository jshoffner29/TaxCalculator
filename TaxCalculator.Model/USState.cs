namespace TaxCalculator.Model
{
    public class USState
    {
        public string StateName { get; set; }
        public string StateCode { get; set; }

        public USState(string state, string stateCode)
        {
            this.StateName = state;
            this.StateCode = stateCode;
        }

        public USState() : this(string.Empty, string.Empty)
        {
        }
    }
}
