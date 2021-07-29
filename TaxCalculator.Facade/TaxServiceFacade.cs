using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Contract;

namespace TaxCalculator.Facade
{
    public class TaxServiceFacade
    {
        private readonly ITaxService taxService;
        private readonly IZipCodeService zipCodeService;
        
        public TaxServiceFacade(ITaxService _taxService, IZipCodeService _zipCodeService)
        {
            taxService = _taxService;
            zipCodeService = _zipCodeService;
        }

        public void StateCodeSelected(string stateCode)
        {

        }
    }
}
