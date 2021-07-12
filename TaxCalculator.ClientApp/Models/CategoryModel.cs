using System.ComponentModel.DataAnnotations;
using TaxCalculator.Model;

namespace TaxCalculator.ClientApp.Models
{
    public class CategoryModel
    {
        public string Name { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage = "Please select a category item")]
        public string ProductTaxCode { get; set; }
        public string Description { get; set; }

        public CategoryModel()
        { }
        public CategoryModel(Category item)
        {
            MapFrom(item);
        }
        public void MapFrom(Category item)
        {
            Name = item.Name;
            ProductTaxCode = item.ProductTaxCode;
            Description = item.Description;
        }
        //public Category MapTo()
        //{
        //    return new Category
        //    {
        //        Name = Name,
        //        ProductTaxCode = ProductTaxCode,
        //        Description = Description
        //    };
        //}
    }
}
