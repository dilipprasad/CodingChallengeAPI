using CodingChallenge.DataLayer.Factories.Interfaces;

namespace CodingChallenge.DataLayer.Factories
{
    public class ObjectDataFactory : IObjectDataFactory
    {
        public IDataLayer _dataLayer { get; }
        public ObjectDataFactory(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }



        public async Task<ICityDataFactory> GetCityDataFactory()
        {
            return (ICityDataFactory)Task.FromResult(new CityDataFactory(_dataLayer));
        }

    }
}
