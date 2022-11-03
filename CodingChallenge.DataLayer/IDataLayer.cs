using CodingChallenge.DataLayer.DTO;

namespace CodingChallenge.DataLayer
{
    public interface IDataLayer
    {
        Task<List<CityDetailsDTO>> GetCityDetails(string zipCode);
    }
}
