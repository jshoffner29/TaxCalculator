using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaxCalculator.ClientApp.Models;
using TaxCalculator.Contract;
using TaxCalculator.Model;
using TaxCalculator.Service;
using Newtonsoft.Json;
using System;

namespace TaxCalculator.ClientApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITaxService _taxService;
        private TaxServiceViewModel _taxServiceViewModel;

        public HomeController(ILogger<HomeController> logger, ITaxService taxService)
        {
            _logger = logger;
            _taxService = taxService;
            _taxServiceViewModel = new TaxServiceViewModel();

            Initialize();
        }

        private void Initialize()
        {
            TaxServiceViewModel.USStates = TaxService.USStates.Select(s => new USStateModel(s)).ToList();
            TaxServiceViewModel.Categories = _taxService
                                                .GetCategories()
                                                .Select(s => new CategoryModel(s))
                                                .ToDictionary(k => k.ProductTaxCode, v => v);
        }

        private void SetViewModel(TaxServiceViewModel model)
        {
            HttpContext.Session.SetObjectAsJson("model", model);
        }

        private TaxServiceViewModel GetViewModel()
        {
            return HttpContext.Session.GetObjectFromJson<TaxServiceViewModel>("model");
        }

        private void ExampleCallingTaxService()
        {
            var us = _taxService.GetUSLocations("HI");
            var result = _taxService.GetTaxRateForLocation(new TaxByLocation { FromZipCode = "90404-3370" });

            var order = new Order
            {
                USLocationFrom = new Model.USLocation
                {
                    Street = "9500 Gilman Drive",
                    City = "La Jolla",
                    StateCode = "CA",
                    ZipCode = "92093"
                },
                USLocationTo = new Model.USLocation
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

            var taxTotal = _taxService.GetTaxForOrder(order);
        }

        public IActionResult Index()
        {
            SetViewModel(_taxServiceViewModel); // store tax service view model's initial state
            var cacheModel = GetViewModel();    // Validate that the object caching is working right.

            cacheModel.SectionStateInstructions = "Start by selecting a state (from the drop-down) and click the search button.";

            return View(cacheModel);
        }
        public IActionResult StateSelected(TaxServiceViewModel model)
        {

            var cacheModel = GetViewModel();

            cacheModel.StateCodeSelected = model.StateCodeSelected;
            cacheModel.ZipCodeSelected = string.Empty;  // clear value, forcing user selection.
            cacheModel.USSlocations = _taxService
                                            .GetUSLocations(cacheModel.StateCodeSelected)
                                            .Select(s => new USLocationModel(s))
                                            .ToList();

            cacheModel.SectionStateInstructions = "Now that a state has been selected, please select a zip code." + Environment.NewLine +
                " Remember, you can always choose a different state.";

            SetViewModel(cacheModel);

            return View("Index", cacheModel);
        }

        public IActionResult ZipCodeSelected(TaxServiceViewModel model, string zipCode)
        {
            var cacheModel = GetViewModel();
            cacheModel.ZipCodeSelected = zipCode;

            cacheModel.SectionTaxForLocationInstructions = "Click 'View Tax Rate' to see the tax rate for this zip code or" +
                " click 'View Zip Codes to make a different selection." + Environment.NewLine +
                "Provide a street address for better accuracy.";

            cacheModel.SectionOrderInstructions = "Create an order by selecting the item's category, quantity, and unit price." + Environment.NewLine +
                    "Click 'Calculate' to view total tax amount for this order.";
            cacheModel.SectionOrderTaxInstructions = "Hover over name and description to see full text.";

            cacheModel.TaxRateForLocation = -1; // indicate the tax rate has not been retrieved

            SetViewModel(cacheModel);
            return View("Index", cacheModel);
        }

        public IActionResult TaxRateForLocation(TaxServiceViewModel model, string submit, string clear)
        {
            var cacheModel = GetViewModel();

            if (!string.IsNullOrEmpty(clear))
            {
                cacheModel.ZipCodeSelected = string.Empty;
            }
            else
            {
                cacheModel.StreetSelected = model.StreetSelected;
                var taxByLocation = new TaxByLocation { FromStreet = cacheModel.StreetSelected, FromZipCode = cacheModel.ZipCodeSelected };
                cacheModel.TaxRateForLocation = _taxService.GetTaxRateForLocation(taxByLocation);

                cacheModel.SectionTaxForLocationInstructions = $"The tax rate for zip code {cacheModel.ZipCodeSelected} is {cacheModel.TaxRateForLocation:P}.";
            }
            SetViewModel(cacheModel);
            return View("Index", cacheModel);
        }
        public IActionResult OrderItemSelected(TaxServiceViewModel model, string add, int removeIndex)
        {
            var cacheModel = GetViewModel();

            if (!string.IsNullOrEmpty(add))
            {
                cacheModel.OrderItemSelected = model.OrderItemSelected;
                cacheModel.AddOrderItem();

            }
            else
            {
                cacheModel.DeleteOrderItem(removeIndex);
            }
            cacheModel.OrderTaxAmount = 0;  // reset order tax amt value.

            cacheModel.SectionOrderTaxInstructions = "Hover over name and description to see full text." + Environment.NewLine +
                "NOTE: Previously selected order items are preserved" +
                " even if the state and zip code is changed!";

            SetViewModel(cacheModel);

            return View("Index", cacheModel);
        }
        public IActionResult OrderTaxAmount()
        {
            var cacheModel = GetViewModel();

            var order = cacheModel.GetOrder().MapTo();
            cacheModel.OrderTaxAmount = _taxService.GetTaxForOrder(order);

            cacheModel.SectionOrderTaxInstructions += $" The total tax for this order is {cacheModel.OrderTaxAmount:C}." + Environment.NewLine;

            SetViewModel(cacheModel);
            return View("Index", cacheModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Help()
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
