using System.Collections.Generic;
using TaxCalculator.Model;

namespace TaxCalculator.Contract
{
    public interface IZipCodeService
    {
        /// <summary>
        /// Get collection of USlocations by zipcode, for the given state.
        /// </summary>
        /// <param name="stateCode">Two-letter ISO state code for given location</param>
        /// <returns></returns>
        IEnumerable<USLocation> GetUSLocations(string stateCode);
    }
}
