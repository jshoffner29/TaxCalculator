using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TaxCalculator.Model;
using TaxCalculator.Service;
using TaxCalculator.Contract;
using Moq;
using TaxCalculator.SupportService;

namespace TaxCalculator.UnitTesting
{
    [TestClass]
    public class IntegrationTesting
    {
        private const string activeClientId = "ActiveClient";
        private ITaxService taxServiceClient;
        private ITaxCalculatorService taxCalculator;
        private IZipCodeService zipCodeService;

        #region environment setup
        [TestInitialize]
        public void TestInitialize()
        {
            taxCalculator = new TaxCalculatorTaxJarService();
            zipCodeService = new ZipCodeService();

            taxServiceClient = new TaxService(activeClientId, taxCalculator, zipCodeService);
        }
        #endregion

        [TestMethod]
        [Description("Validate the list of state name and state code associations")]
        public void ValidateUSStatesNameAndCode()
        {
            // Arrange
            var allStates = TaxService.USStates;
            var errorStates = new List<USState>();

            // Act
            foreach (var state in allStates)
            {
                var targetState = state;

                var lookupState = taxServiceClient.GetUSLocations(state.StateCode).FirstOrDefault();

                if (lookupState == null ||
                    targetState.StateName != lookupState.StateName)
                {
                    errorStates.Add(targetState);
                }
            }

            // Assert
            Assert.IsNotNull(errorStates);
            Assert.IsFalse(errorStates.Any(),$"There are {errorStates.Count} non-matching state names");
        }
    }
}
