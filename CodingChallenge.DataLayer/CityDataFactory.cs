using CodingChallenge.DataLayer.DTO;

namespace CodingChallenge.DataLayer
{
    public class CityDataFactory : ICityDataFactory
    {
        private IDataLayer _dataLayer { get; }

        public CityDataFactory(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }


        public async Task<CityDetailsDTO> GetZipCodeByCity(string zipCode)
        {
           return   _dataLayer.GetCityDetails(zipCode).Result.First();// Returning first data temporarily
            
        }
    }
}
