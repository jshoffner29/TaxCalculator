using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TaxCalculator.Contract;
using TaxCalculator.Model;
using TaxCalculator.SupportService;

namespace TaxCalculator.UnitTesting
{
    [TestClass]
    public class ZipCodeTesting
    {
        private IZipCodeService zipCodeService;

        #region environment setup
        [TestInitialize]
        public void TestInitialize()
        {
            zipCodeService = new ZipCodeService();
        }
        #endregion
        [TestMethod]
        public void GetUSLocations_StateCodeNotFound()
        {
            // Arrange
            Dictionary<string, string> stateCodeAndErrorMessage = new()
            {
                { "01","State Code does not exist" },
                { "","State Code string is empty" },
                { " ","State Code string has whitespace" }
            };
            var stateCodeIsNull = (string)null;
            var stateCodeIsNullErrorMessage = "State Code string was not set (is null)";
            var errorMessages = new List<string>(stateCodeAndErrorMessage.Count + 1);

            // Act - Validate no value state codes
            foreach (var stateCodeItem in stateCodeAndErrorMessage)
            {
                try
                {
                    zipCodeService.GetUSLocations(stateCodeItem.Key);
                }
                catch (ArgumentNullException)
                {
                    errorMessages.Add(stateCodeItem.Value);
                }
            }

            // Act - Validate state code is not null
            try
            {
                zipCodeService.GetUSLocations(stateCodeIsNull);
            }
            catch (ArgumentNullException)
            {
                errorMessages.Add(stateCodeIsNullErrorMessage);
            }

            // Assert
            Assert.IsNotNull(errorMessages);
            Assert.IsTrue(errorMessages.Any());
            Assert.AreEqual(stateCodeAndErrorMessage.Count + 1, errorMessages.Count, "Error messages in Dict and state code is null error msg.");
            Assert.IsTrue(errorMessages.Any(a => Equals(a, stateCodeAndErrorMessage["01"])));
            Assert.IsTrue(errorMessages.Any(a => Equals(a, stateCodeAndErrorMessage[""])));
            Assert.IsTrue(errorMessages.Any(a => Equals(a, stateCodeAndErrorMessage[" "])));
            Assert.IsTrue(errorMessages.Any(a => Equals(a, stateCodeIsNullErrorMessage)));
        }
        [TestMethod]
        public void GetUSLocations_ValidateEntityIsSet()
        {
            // Arrange - using my home town
            const string stateCode = "HI";
            const string zipCode = "96744";
            var expectedResult = new USLocation
            {
                Street = string.Empty,
                City = "Kaneohe",
                StateName = "Hawaii",
                StateCode = "HI",
                ZipCode = zipCode,
                Lat = 21.4228,
                Lng = -157.8115
            };

            // Act
            var results = zipCodeService.GetUSLocations(stateCode).First(s => s.ZipCode.Equals(zipCode));

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(expectedResult.Street, results.Street);
            Assert.AreEqual(expectedResult.City, results.City);
            Assert.AreEqual(expectedResult.StateName, results.StateName);
            Assert.AreEqual(expectedResult.StateCode, results.StateCode);
            Assert.AreEqual(expectedResult.ZipCode, results.ZipCode);
            Assert.AreEqual(expectedResult.Lat, results.Lat);
            Assert.AreEqual(expectedResult.Lng, results.Lng);
        }
    }
}
