using AutoMapper;
using CodingChallenge.Business.Interfaces;
using CodingChallenge.DataLayer;
using CodingChallenge.DataLayer.Factories.Interfaces;
using CodingChallenge.Logging.Interface;
using CodingChallenge.Models;

namespace CodingChallenge.Business
{
    public class CityBusinessProvider : ICityBusinessProvider
    {
        private IObjectDataFactory _objectDataFactory { get; }
        private ICityDataFactory _cityDatafactory;

        public IMapper Mapper
        {
            get { return MapperSetup.ObjectMapper.Mapper; }
        }
        public CityBusinessProvider(ILogging logger, IObjectDataFactory objectDataFactory)
        {
            _objectDataFactory = objectDataFactory;
            _cityDatafactory =  _objectDataFactory.GetCityDataFactory().Result;
        }

        public async Task<CityDetails> GetZipCodeByCity(string zipCode)
        {
            var result= await _cityDatafactory.GetZipCodeByCity(zipCode);
            return  Mapper.Map<CityDetails>(result);
        }
    }
}