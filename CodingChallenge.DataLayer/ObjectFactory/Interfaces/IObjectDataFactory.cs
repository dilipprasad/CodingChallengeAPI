namespace CodingChallenge.DataLayer.Factories.Interfaces
{
    public interface IObjectDataFactory
    {
        Task<ICityDataFactory> GetCityDataFactory();
    }
}
