using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TaxCalculator.Contract;
using TaxCalculator.Service;

namespace TaxCalculator.UnitTesting
{
    [TestClass]
    public class TaxServiceTesting
    {
        private const string activeClientId = "ActiveClient";
        private ITaxService taxServiceClient;
        private Mock<ITaxCalculatorService> taxCalculatorService;
        private Mock<IZipCodeService> zipCodeService;

        #region environment setup
        [TestInitialize]
        public void TestInitialize()
        {
            taxCalculatorService = new Mock<ITaxCalculatorService>();
            zipCodeService = new Mock<IZipCodeService>();

            taxServiceClient = new TaxService(activeClientId, taxCalculatorService.Object, zipCodeService.Object);
        }
        #endregion

        [TestMethod]
        public void TaxService_ValidateInitialization()
        {
            // Arrange - no variables

            // Act - act happened at test initialization

            // Assert
            Assert.IsNotNull(taxServiceClient);
            Assert.IsNotNull(taxServiceClient.ClientId);

            taxCalculatorService.Verify(v => v.Initialize(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InitializeTaxCalculator_UnSupportedClient()
        {
            // Arrange
            const string clientId = "UnSupportedClient";
            var taxCalculatorService = new Mock<ITaxCalculatorService>();
            var zipCodeService = new Mock<IZipCodeService>();

            // Act
            new TaxService(clientId, taxCalculatorService.Object, zipCodeService.Object);

            // Assert - no assertion, should throw exception            
        }

        [TestMethod]
        public void InitializeTaxCalculator_DefaultClient()
        {
            // Arrange
            const string clientId = "ClientNotSetUp";
            var taxCalculatorService = new Mock<ITaxCalculatorService>();
            var zipCodeService = new Mock<IZipCodeService>();

            // Act
            var results = new TaxService(clientId, taxCalculatorService.Object, zipCodeService.Object);

            // Assert
            taxCalculatorService.Verify(v => v.Initialize(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(results);
        }
    }
}