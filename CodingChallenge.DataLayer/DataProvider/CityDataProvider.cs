using CodingChallenge.DataLayer.DataAdaptor;
using CodingChallenge.DataLayer.DataProvider.Interfaces;
using CodingChallenge.DataLayer.DTO;

namespace CodingChallenge.DataLayer.DataProvider
{
    public class CityDataProvider : ICityDataProvider
    {
        private IDataLayer _dataLayer { get; }

        public CityDataProvider(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }


        public async Task<List<CityDetailsDTO>> GetZipCodeByCity(string zipCode)
        {
            return await _dataLayer.SearchCityDetailsByZipCode(zipCode);

        }
    }
}
