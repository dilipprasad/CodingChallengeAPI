using AutoMapper;
using CodingChallenge.Business.Interfaces;
using CodingChallenge.DataLayer.DataProvider.Interfaces;
using CodingChallenge.DataLayer.ObjectFactory.Interfaces;
using CodingChallenge.Logging.Interface;
using CodingChallenge.Models;

namespace CodingChallenge.Business
{
    public class CityBusinessProvider : ICityBusinessProvider
    {
        public ILogging<CityBusinessProvider> _logger { get; }
        private IObjectDataFactory _objectDataFactory { get; }
        private ICityDataProvider _cityDataProvider;

        public IMapper Mapper
        {
            get { return MapperSetup.ObjectMapper.Mapper; }
        }
        public CityBusinessProvider(CodingChallenge.Logging.Interface.ILogging<CityBusinessProvider> logger, IObjectDataFactory objectDataFactory)
        {
            _logger = logger;
            _objectDataFactory = objectDataFactory;
            var res = _objectDataFactory.GetCityDataFactory().Result;//This causes Threading issue, so always use synchronous object creation 
            _cityDataProvider = (ICityDataProvider) res;
        }

        public async Task<List<CityDetails>> GetZipCodeByCity(string zipCode)
        {

            var result= await _cityDataProvider.GetZipCodeByCity(zipCode);
            return  Mapper.Map<List<CityDetails>>(result);
        }
    }
}