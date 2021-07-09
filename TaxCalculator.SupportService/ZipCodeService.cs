using SimpleZipCode;
using System;
using System.Collections.Generic;
using System.Linq;
using TaxCalculator.Contract;
using TaxCalculator.Model;

namespace TaxCalculator.SupportService
{
    public class ZipCodeService : IZipCodeService
    {
        private readonly IZipCodeRepository zipCodeRepository;
        public ZipCodeService()
        {
            zipCodeRepository = ZipCodeSource.FromMemory().GetRepository();
        }

        /// <summary>
        /// Two-letter ISO state code
        /// </summary>
        /// <param name="stateCode">Two-letter ISO state code</param>
        /// <returns>USLocation entity</returns>
        public IEnumerable<USLocation> GetUSLocations(string stateCode)
        {
            var results = zipCodeRepository.Search(x => x.StateAbbreviation.Equals(stateCode));

            if (results == null || !results.Any())
                throw new ArgumentNullException(nameof(stateCode), $"Unable to retrieve zip code from the state abbreviation of {stateCode}");

            return results.Select(s =>
                new USLocation
                {
                    Street = string.Empty,
                    City = s.PlaceName,
                    StateName = s.State,
                    StateCode = s.StateAbbreviation,
                    ZipCode = s.PostalCode,
                    Lat = s.Latitude,
                    Lng = s.Longitude
                });
        }
    }
}
