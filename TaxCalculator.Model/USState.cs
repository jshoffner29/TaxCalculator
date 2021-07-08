namespace TaxCalculator.Model
{
    public class USState
    {
        public string StateName { get; set; }
        public string StateCode { get; set; }

        public USState(string stateName, string stateCode)
        {
            this.StateName = stateName;
            this.StateCode = stateCode;
        }

        public USState() : this(string.Empty, string.Empty)
        {
        }
    }
}
