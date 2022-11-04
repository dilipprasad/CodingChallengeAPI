using CodingChallenge.DataLayer.DataProvider.Interfaces;

namespace CodingChallenge.DataLayer.ObjectFactory.Interfaces
{
    public interface IObjectDataFactory
    {
        Task<ICityDataProvider> GetCityDataFactory();
    }
}
