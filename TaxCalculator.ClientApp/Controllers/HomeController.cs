using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TaxCalculator.ClientApp.Models;
using TaxCalculator.Contract;

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
            TaxServiceViewModel.SetLookUpValues(taxService);
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

        public IActionResult ZipCodeFiltering(TaxServiceViewModel model, bool clear)
        {
            var cacheModel = GetViewModel();

            if(clear)
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

        public IActionResult TaxRateForLocation(TaxServiceViewModel model, bool viewZipCode)
        {
            var cacheModel = GetViewModel();

            if (viewZipCode)
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