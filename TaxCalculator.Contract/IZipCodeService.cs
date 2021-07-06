using System.Collections.Generic;
using TaxCalculator.Model;

namespace TaxCalculator.Contract
{
    public interface IZipCodeService
    {
        IEnumerable<USLocation> GetUSLocations(string stateCode);
    }
}
