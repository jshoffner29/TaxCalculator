using TaxCalculator.Model;

namespace TaxCalculator.ClientApp.Models
{
    public class USLocationModel
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public USLocationModel()
        { }

        public USLocationModel(USLocation item)
        {
            MapFrom(item);
        }

        public void MapFrom(USLocation item)
        {
            Street = item.Street;
            City = item.City;
            StateCode = item.StateCode;
            ZipCode = item.ZipCode;
            Country = item.Country;
            Lat = item.Lat;
            Lng = item.Lng;
        }

        public USLocation MapTo()
        {
            return new USLocation
            {
                Street = Street,
                City = City,
                StateCode = StateCode,
                ZipCode = ZipCode,
                Lat = Lat,
                Lng = Lng
            };
        }
    }
}
