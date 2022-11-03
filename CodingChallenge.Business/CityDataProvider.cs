using CodingChallenge.Business.Interfaces;
using CodingChallenge.DataLayer;
using CodingChallenge.DataLayer.Factories.Interfaces;
using CodingChallenge.Logging.Interface;
using CodingChallenge.Models;

namespace CodingChallenge.Business
{
    public class CityDataProvider : ICityDataProvider
    {
        private IObjectDataFactory _objectDataFactory { get; }
        private ICityDataFactory _cityDatafactory;

        public CityDataProvider(ILogging logger, IObjectDataFactory objectDataFactory)
        {
            _objectDataFactory = objectDataFactory;
            _cityDatafactory =  _objectDataFactory.GetCityDataFactory().Result;
        }

        public async Task<CityDetails> GetZipCodeByCity(string zipCode)
        {
            return await _cityDatafactory.GetZipCodeByCity(zipCode);
        }
    }
}