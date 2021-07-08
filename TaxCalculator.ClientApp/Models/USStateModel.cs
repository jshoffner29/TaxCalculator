using TaxCalculator.Model;

namespace TaxCalculator.ClientApp.Models
{
    public class USStateModel
    {
        public string StateName { get; set; }
        public string StateCode { get; set; }

        public USStateModel()
        { }
        public USStateModel(USState item)
        { MapFrom(item); }
        public void MapFrom(USState item)
        {
            StateName = item.StateName;
            StateCode = item.StateCode;
        }
        public USState MapTo()
        {
            return new USState(StateName, StateCode);
        }
    }
}
