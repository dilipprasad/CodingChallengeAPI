using CodingChallenge.DataLayer.DTO;

namespace CodingChallenge.DataLayer.DataAdaptor
{
    public interface IDataLayer
    {
        Task<List<CityDetailsDTO>> SearchCityDetailsByZipCode(string zipCode);

        Task<List<CityDetailsDTO>> SearchCityDetailsByName(string cityName);
    }
}
