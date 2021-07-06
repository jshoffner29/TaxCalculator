using TaxCalculator.Contract;

namespace TaxCalculator.Service
{
    public class TaxService : TaxServiceBase
    {
        public TaxService(
            string clientId,
            ITaxCalculatorService _taxCalculator,
            IZipCodeService _zipCodeService) : base(clientId, _taxCalculator, _zipCodeService)
        { }
    }
}
