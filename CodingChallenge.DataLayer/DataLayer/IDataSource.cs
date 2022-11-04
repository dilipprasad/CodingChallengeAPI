using CodingChallenge.DataLayer.DTO;

namespace CodingChallenge.DataLayer.DataLayer
{
    public interface IDataSource
    {
        Task<List<CityDetailsDTO>> GetCitySource();
    }
}