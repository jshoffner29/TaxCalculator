using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaxCalculator.ClientApp.Models;
using TaxCalculator.Contract;
using TaxCalculator.Model;
using TaxCalculator.Service;

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
            model.SetInstructions();
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

            cacheModel.SetInstructions();

            return View(cacheModel);
        }
        public IActionResult StateSelected(TaxServiceViewModel model)
        {

            var cacheModel = GetViewModel();

            cacheModel.SetStateCode(_taxService, model.StateCodeSelected);

            SetViewModel(cacheModel);

            return View("Index", cacheModel);
        }

        public IActionResult ZipCodeFiltering(TaxServiceViewModel model, string clear)
        {
            var cacheModel = GetViewModel();

            if(!string.IsNullOrEmpty(clear))
            {
                model.FilteredCityNameSelected = string.Empty;
                model.FilteredZipCodeSelected = string.Empty;
            }

            cacheModel.SetFilteredUSLocations(model.FilteredCityNameSelected, model.FilteredZipCodeSelected);
            
            SetViewModel(cacheModel);

            return View("Index", cacheModel);
        }


        public IActionResult ZipCodeSelected(TaxServiceViewModel model, string zipCode)
        {
            var cacheModel = GetViewModel();

            cacheModel.ZipCodeSelected = zipCode;
            cacheModel.TaxRateForLocation = -1; // reset tax rate value

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
            cacheModel.OrderTaxAmount = -1;  // reset order tax amt value.

            SetViewModel(cacheModel);

            return View("Index", cacheModel);
        }
        public IActionResult OrderTaxAmount()
        {
            var cacheModel = GetViewModel();

            var order = cacheModel.GetOrder().MapTo();
            cacheModel.OrderTaxAmount = _taxService.GetTaxForOrder(order);

            SetViewModel(cacheModel);
            return View("Index", cacheModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Features()
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
