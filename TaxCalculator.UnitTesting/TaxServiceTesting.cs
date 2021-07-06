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
    public class TaxServiceTesting
    {
        private const string activeClientId = "ActiveClient";
        private ITaxService taxServiceClient;
        private Mock<ITaxCalculatorService> taxCalculator;
        private Mock<IZipCodeService> zipCodeService;

        #region environment setup
        [TestInitialize]
        public void TestInitialize()
        {
            taxCalculator = new Mock<ITaxCalculatorService>();
            zipCodeService = new Mock<IZipCodeService>();

            taxServiceClient = new TaxService(activeClientId, taxCalculator.Object, zipCodeService.Object);
        }
        #endregion

    }
}
