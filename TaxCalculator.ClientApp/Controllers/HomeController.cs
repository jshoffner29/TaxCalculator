using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.ClientApp.Models;
using TaxCalculator.Model;
using TaxCalculator.Service;
using TaxCalculator.SupportService;

namespace TaxCalculator.ClientApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var s = new TaxService("client 1", new TaxCalculatorTaxJarService(), new ZipCodeService());

            var result = s.GetTaxRateForLocation(new TaxByLocation { FromZipCode = "90404-3370" });

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
                    new OrderLineItem { Quanitity = 2, UnitPrice = 7 }
                }
            };

            var taxTotal = s.GetTaxForOrder(order);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
