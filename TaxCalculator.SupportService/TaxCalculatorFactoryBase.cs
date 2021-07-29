using System;
using System.Collections.Generic;
using TaxCalculator.Contract;
using TaxCalculator.Model;

namespace TaxCalculator.SupportService
{
    public abstract class TaxCalculatorFactoryBase : ITaxCalculatorFactory
    {
        public virtual decimal GetTaxForOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public virtual decimal GetTaxRateForLocation(TaxByLocation taxByLocation)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<Model.Category> GetCategories()
        {
            throw new NotImplementedException();
        }
    }
}