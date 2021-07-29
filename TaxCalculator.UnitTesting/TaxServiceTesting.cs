using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaxCalculator.Contract;
using TaxCalculator.Service;

namespace TaxCalculator.UnitTesting
{
    [TestClass]
    public class TaxServiceTesting
    {
        private ITaxService taxServiceClient;
        private Mock<ITaxCalculatorFactory> taxCalculatorService;

        #region environment setup
        [TestInitialize]
        public void TestInitialize()
        {
            taxCalculatorService = new Mock<ITaxCalculatorFactory>();

            taxServiceClient = new TaxService(taxCalculatorService.Object);
        }
        #endregion
    }
}