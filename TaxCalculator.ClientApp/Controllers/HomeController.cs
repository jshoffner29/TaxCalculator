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
        private readonly ITaxService taxService;
        private readonly IZipCodeService zipCodeService;
        private TaxServiceViewModel _taxServiceViewModel;

        public HomeController(ILogger<HomeController> logger, ITaxService taxService, IZipCodeService zipCodeService)
        {
            _logger = logger;
            this.taxService = taxService;
            this.zipCodeService = zipCodeService;
            _taxServiceViewModel = new TaxServiceViewModel();

            Initialize();
        }

        private void Initialize()
        {
            TaxServiceViewModel.USStates = TaxService.USStates.Select(s => new USStateModel(s)).ToList();
            TaxServiceViewModel.Categories = taxService
                                                .GetCategories()
                                                .Select(s => new CategoryModel(s))
                                                .ToDictionary(k => k.ProductTaxCode, v => v);
        }

        private void SetViewModel(TaxServiceViewModel model)
        {
            model.SetModel(taxService);
            HttpContext.Session.SetObjectAsJson("model", model);
        }

        private TaxServiceViewModel GetViewModel()
        {
            return HttpContext.Session.GetObjectFromJson<TaxServiceViewModel>("model");
        }

        //private void ExampleCallingTaxService()
        //{
        //    var us = _zipCodeService.GetUSLocations("HI");
        //    var result = _taxService.GetTaxRateForLocation(new TaxByLocation { FromZipCode = "90404-3370" });

        //    var order = new Order
        //    {
        //        USLocationFrom = new Model.USLocation
        //        {
        //            Street = "9500 Gilman Drive",
        //            City = "La Jolla",
        //            StateCode = "CA",
        //            ZipCode = "92093"
        //        },
        //        USLocationTo = new Model.USLocation
        //        {
        //            Street = "1335 E 103rd St",
        //            City = "Los Angeles",
        //            StateCode = "CA",
        //            ZipCode = "90002"
        //        },
        //        LineItems = new List<OrderLineItem>
        //        {
        //            new OrderLineItem { Quanitity = 2, UnitPrice = 7 }
        //        }
        //    };

        //    var taxTotal = _taxService.GetTaxForOrder(order);
        //}

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

            cacheModel.SetStateCode(zipCodeService, model.StateCodeSelected);

            SetViewModel(cacheModel);

            return View("Index", cacheModel);
        }

        public IActionResult ZipCodeFiltering(TaxServiceViewModel model, string clear)
        {
            var cacheModel = GetViewModel();

            if(!string.IsNullOrEmpty(clear))
            {
                model.ClearFilteredUSLocations();
            }

            cacheModel.SetFilteredUSLocations(model.FilteredCityNameSelected, model.FilteredZipCodeSelected);
            
            SetViewModel(cacheModel);

            return View("Index", cacheModel);
        }


        public IActionResult ZipCodeSelected(TaxServiceViewModel model, string zipCode)
        {
            var cacheModel = GetViewModel();

            cacheModel.SetZipCode(zipCode);

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
                cacheModel.SetTaxRateForLocation(taxService, model.StreetSelected);
            }

            SetViewModel(cacheModel);
            return View("Index", cacheModel);
        }
        public IActionResult OrderItemSelected(TaxServiceViewModel model, string add, int removeIndex)
        {
            var cacheModel = GetViewModel();
            cacheModel.OrderItemSelected = model.OrderItemSelected;

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(add))
                {
                    
                    cacheModel.AddOrderItem();
                }
                else
                {
                    cacheModel.DeleteOrderItem(removeIndex);
                }                
            }

            SetViewModel(cacheModel);

            return View("Index", cacheModel);
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
