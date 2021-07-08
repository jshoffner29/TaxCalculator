using TaxCalculator.Contract;

namespace TaxCalculator.Service
{
    public class TaxService : TaxServiceBase
    {
        public TaxService(
            ITaxCalculatorService _taxCalculator,
            IZipCodeService _zipCodeService,
            string clientId = "Default") : base(_taxCalculator, _zipCodeService, clientId)
        { }
    }
}
