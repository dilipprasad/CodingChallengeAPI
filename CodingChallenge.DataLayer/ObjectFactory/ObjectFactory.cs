using CodingChallenge.DataLayer.DataAdaptor;
using CodingChallenge.DataLayer.DataProvider;
using CodingChallenge.DataLayer.DataProvider.Interfaces;
using CodingChallenge.DataLayer.ObjectFactory.Interfaces;

namespace CodingChallenge.DataLayer.ObjectFactory
{
    public class ObjectDataFactory : IObjectDataFactory
    {
        public IDataLayer _dataLayer { get; }
        public ObjectDataFactory(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }



        public async Task<ICityDataProvider> GetCityDataFactory()
        {
            //return Task.FromResult<ICityDataProvider>(new CityDataProvider(_dataLayer));
            return new CityDataProvider(_dataLayer);
        }

    }
}
