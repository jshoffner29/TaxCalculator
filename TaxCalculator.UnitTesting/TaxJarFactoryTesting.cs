using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TaxCalculator.Contract;
using TaxCalculator.Model;
using TaxCalculator.Service;
using TaxCalculator.SupportService;

namespace TaxCalculator.UnitTesting
{
    [TestClass]
    public class TaxJarFactoryTesting
    {
        private ITaxCalculatorFactory taxCalculatorTaxJar;

        #region environment setup
        [TestInitialize]
        public void TestInitialize()
        {
            taxCalculatorTaxJar = new TaxJarFactory();
        }
        #endregion
        [TestMethod]
        public void GetTaxForOrder_ValidateRequiredFields()
        {
            // Arrange
            var t1 = new Order();
            var t2 = new Order
            {
                USLocationFrom = new USLocation()
            };
            var t3 = new Order
            {
                USLocationTo = new USLocation()
            };
            var t4 = new Order
            {
                USLocationFrom = new USLocation { StateCode = "CA", ZipCode = "92093" },
                USLocationTo = new USLocation()
            };
            var t5 = new Order
            {
                USLocationFrom = new USLocation(),
                USLocationTo = new USLocation { StateCode = "CA", ZipCode = "90002" }
            };
            var t6 = new Order
            {
                USLocationFrom = new USLocation { StateCode = "CA", ZipCode = "92093" },
                USLocationTo = new USLocation { StateCode = "CA", ZipCode = "90002" }
            };
            Dictionary<Order, string> orderAndErrorMessage = new()
            {
                { t1, "No Fields Set" },
                { t2, "Location TO Not Set" },
                { t3, "Location FROM Not Set" },
                { t4, "Location TO Missing Required fields" },
                { t5, "Location FROM Missing Required fields" },
                { t6, "Line Items Missing" }
            };
            var orderIsNull = (Order)null;
            var orderIsNullErrorMessage = "The order was not set (is null)";
            var errorMessages = new List<string>(orderAndErrorMessage.Count + 1);

            // Act - Validate order
            foreach (var order in orderAndErrorMessage)
            {
                try
                {
                    taxCalculatorTaxJar.GetTaxForOrder(order.Key);
                }
                catch (ArgumentNullException)
                {
                    errorMessages.Add(order.Value);
                }
            }

            // Act - Validate order is not null
            try
            {
                taxCalculatorTaxJar.GetTaxForOrder(orderIsNull);
            }
            catch (ArgumentNullException)
            {
                errorMessages.Add(orderIsNullErrorMessage);
            }

            // Assert
            Assert.IsNotNull(errorMessages);
            Assert.IsTrue(errorMessages.Any());
            Assert.AreEqual(orderAndErrorMessage.Count + 1, errorMessages.Count, "Error messages in Dict and state code is null error msg.");
            Assert.IsTrue(errorMessages.Any(a => Equals(a, orderAndErrorMessage[t1])));
            Assert.IsTrue(errorMessages.Any(a => Equals(a, orderAndErrorMessage[t2])));
            Assert.IsTrue(errorMessages.Any(a => Equals(a, orderAndErrorMessage[t3])));
            Assert.IsTrue(errorMessages.Any(a => Equals(a, orderAndErrorMessage[t4])));
            Assert.IsTrue(errorMessages.Any(a => Equals(a, orderAndErrorMessage[t5])));
            Assert.IsTrue(errorMessages.Any(a => Equals(a, orderAndErrorMessage[t6])));
            Assert.IsTrue(errorMessages.Any(a => Equals(a, orderIsNullErrorMessage)));
        }
        [TestMethod]
        [Description("Verify that a tax amount was received (dollar amt will change over time).")]
        public void GetTaxForOrder_HappyPath()
        {
            // Arrange
            var order = new Order
            {
                USLocationFrom = new USLocation
                {
                    Street = "9500 Gilman Drive",
                    City = "La Jolla",
                    StateCode = "CA",
                    ZipCode = "92093"
                },
                USLocationTo = new USLocation
                {
                    Street = "1335 E 103rd St",
                    City = "Los Angeles",
                    StateCode = "CA",
                    ZipCode = "90002"
                },
                LineItems = new List<OrderLineItem>
                {
                    new OrderLineItem { Quanitity = 1, UnitPrice = 15 },
                    new OrderLineItem { Quanitity = 2, UnitPrice = 7 }
                }
            };

            // Act
            var results = taxCalculatorTaxJar.GetTaxForOrder(order);

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results > 0, "Verify that a tax amount was received.");
        }
        [TestMethod]
        [Description("Verify that a tax amount was received (dollar amt will change over time).")]
        public void GetTaxForOrder_HappyPathNoFromStreetOrCity()
        {
            // Arrange
            var order = new Order
            {
                USLocationFrom = new USLocation
                {
                    StateCode = "CA",
                    ZipCode = "92093"
                },
                USLocationTo = new USLocation
                {
                    Street = "1335 E 103rd St",
                    City = "Los Angeles",
                    StateCode = "CA",
                    ZipCode = "90002"
                },
                LineItems = new List<OrderLineItem>
                {
                    new OrderLineItem { Quanitity = 1, UnitPrice = 15 },
                    new OrderLineItem { Quanitity = 2, UnitPrice = 7 }
                }
            };

            // Act
            var results = taxCalculatorTaxJar.GetTaxForOrder(order);

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results > 0, "Verify that a tax amount was received.");
        }
        [TestMethod]
        public void GetTaxForOrder_ValidateFromMustBeInSameState()
        {
            // Arrange
            var order = new Order
            {
                USLocationFrom = new USLocation
                {
                    StateCode = "HI",
                    ZipCode = "96744"
                },
                USLocationTo = new USLocation
                {
                    Street = "1335 E 103rd St",
                    City = "Los Angeles",
                    StateCode = "CA",
                    ZipCode = "90002"
                },
                LineItems = new List<OrderLineItem>
                {
                    new OrderLineItem { Quanitity = 1, UnitPrice = 15 },
                    new OrderLineItem { Quanitity = 2, UnitPrice = 7 }
                }
            };

            // Act
            var results = taxCalculatorTaxJar.GetTaxForOrder(order);

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results == 0, "The tax amount could not be calculated.");
        }
        [TestMethod]
        [Description("Verify that a tax amount was received (dollar amt will change over time).")]
        public void GetTaxForOrder_ValidateTaxIsTheSameRegardlessOfFromLocation()
        {
            // Arrange
            var order1 = new Order
            {
                USLocationFrom = new USLocation
                {
                    StateCode = "CA",
                    ZipCode = "92093"
                },
                USLocationTo = new USLocation
                {
                    Street = "1335 E 103rd St",
                    City = "Los Angeles",
                    StateCode = "CA",
                    ZipCode = "90002"
                },
                LineItems = new List<OrderLineItem>
                {
                    new OrderLineItem { Quanitity = 1, UnitPrice = 15 },
                    new OrderLineItem { Quanitity = 2, UnitPrice = 7 }
                }
            };
            var order2 = new Order
            {
                USLocationFrom = new USLocation
                {
                    StateCode = "CA",
                    ZipCode = "91030"
                },
                USLocationTo = new USLocation
                {
                    Street = "1335 E 103rd St",
                    City = "Los Angeles",
                    StateCode = "CA",
                    ZipCode = "90002"
                },
                LineItems = new List<OrderLineItem>
                {
                    new OrderLineItem { Quanitity = 1, UnitPrice = 15 },
                    new OrderLineItem { Quanitity = 2, UnitPrice = 7 }
                }
            };
            var order3 = new Order
            {
                USLocationFrom = new USLocation
                {
                    StateCode = "CA",
                    ZipCode = "90744"
                },
                USLocationTo = new USLocation
                {
                    Street = "1335 E 103rd St",
                    City = "Los Angeles",
                    StateCode = "CA",
                    ZipCode = "90002"
                },
                LineItems = new List<OrderLineItem>
                {
                    new OrderLineItem { Quanitity = 1, UnitPrice = 15 },
                    new OrderLineItem { Quanitity = 2, UnitPrice = 7 }
                }
            };
            var order4 = new Order
            {
                USLocationFrom = new USLocation
                {
                    StateCode = "CA",
                    ZipCode = "90002"
                },
                USLocationTo = new USLocation
                {
                    Street = "1335 E 103rd St",
                    City = "Los Angeles",
                    StateCode = "CA",
                    ZipCode = "90002"
                },
                LineItems = new List<OrderLineItem>
                {
                    new OrderLineItem { Quanitity = 1, UnitPrice = 15 },
                    new OrderLineItem { Quanitity = 2, UnitPrice = 7 }
                }
            };

            // Act
            var results1 = taxCalculatorTaxJar.GetTaxForOrder(order1);
            var results2 = taxCalculatorTaxJar.GetTaxForOrder(order2);
            var results3 = taxCalculatorTaxJar.GetTaxForOrder(order3);
            var results4 = taxCalculatorTaxJar.GetTaxForOrder(order4);

            // Assert
            Assert.IsNotNull(results1);
            Assert.IsTrue(results1 > 0, "Verify that a tax amount was received.");
            Assert.IsNotNull(results2);
            Assert.IsTrue(results1 > 0, "Verify that a tax amount was received.");
            Assert.IsNotNull(results3);
            Assert.IsTrue(results1 > 0, "Verify that a tax amount was received.");
            Assert.IsNotNull(results4);
            Assert.IsTrue(results1 > 0, "Verify that a tax amount was received.");

            Assert.AreEqual(results1, results2);
            Assert.AreEqual(results1, results3);
            Assert.AreEqual(results1, results4);
        }
        [TestMethod]
        public void GetTaxRateForLocation_FailsValidation()
        {
            // Arrange
            var t1 = new TaxByLocation { FromZipCode = "00110" };
            var t2 = new TaxByLocation { FromZipCode = null };
            var t3 = new TaxByLocation { FromZipCode = string.Empty };
            var t4 = new TaxByLocation { FromZipCode = " " };

            Dictionary<TaxByLocation, string> taxByLocationAndErrorMessage = new()
            {
                { t1, "Zipcode does not exist" },
                { t2, "Zipcode is null" },
                { t3, "Zipcode is empty" },
                { t4, "Zipcode string has whitespace" }
            };

            var taxByLocationIsNull = (TaxByLocation)null;
            var taxByLocationIsNullErrorMessage = "Zipcode string was not set (is null)";
            var errorMessages = new List<string>(taxByLocationAndErrorMessage.Count + 1);

            // Act - validate zipcode values
            foreach (var taxByLocation in taxByLocationAndErrorMessage)
            {
                try
                {
                    taxCalculatorTaxJar.GetTaxRateForLocation(taxByLocation.Key);
                }
                catch (SystemException)
                {
                    errorMessages.Add(taxByLocation.Value);
                }
            }

            // Act - validate tax by location is not null
            try
            {
                taxCalculatorTaxJar.GetTaxRateForLocation(taxByLocationIsNull);
            }
            catch (ArgumentNullException)
            {
                errorMessages.Add(taxByLocationIsNullErrorMessage);
            }

            // Assert
            Assert.IsNotNull(errorMessages);
            Assert.IsTrue(errorMessages.Any());
            Assert.AreEqual(taxByLocationAndErrorMessage.Count + 1, errorMessages.Count, "Error messages in Dict and zipcode is null error msg.");
            Assert.IsTrue(errorMessages.Any(a => Equals(a, taxByLocationAndErrorMessage[t1])));
            Assert.IsTrue(errorMessages.Any(a => Equals(a, taxByLocationAndErrorMessage[t2])));
            Assert.IsTrue(errorMessages.Any(a => Equals(a, taxByLocationAndErrorMessage[t3])));
            Assert.IsTrue(errorMessages.Any(a => Equals(a, taxByLocationAndErrorMessage[t4])));
            Assert.IsTrue(errorMessages.Any(a => Equals(a, taxByLocationIsNullErrorMessage)));
        }
        [TestMethod]
        [Description("Validates that a tax rate is returned for this zip code (rates change over time).")]
        public void GetTaxRateForLocation_ValidateTaxRateReturnedForZipCodeOnly()
        {
            // Arrange
            var taxByLocation = new TaxByLocation { FromZipCode = "90404-3370" };

            // Act
            var results = taxCalculatorTaxJar.GetTaxRateForLocation(taxByLocation);

            // Act
            Assert.IsNotNull(results);

        }
        [TestMethod]
        [Description("Validates that a tax rate is returned for this zip code and street addr (rates change over time).")]
        public void GetTaxRateForLocation_ValidateTaxRateReturnedForZipCodeAndStreet()
        {
            // Arrange
            var taxByLocation = new TaxByLocation { FromZipCode = "33810", FromStreet = "1708 Altavista Cir" };

            // Act
            var results = taxCalculatorTaxJar.GetTaxRateForLocation(taxByLocation);

            // Act
            Assert.IsNotNull(results);

        }
        [TestMethod]
        public void GetCategories_ValidateEntitiesAreReturned()
        {
            // Arrange - no variables

            // Act
            var results = taxCalculatorTaxJar.GetCategories();

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }
    }
}
