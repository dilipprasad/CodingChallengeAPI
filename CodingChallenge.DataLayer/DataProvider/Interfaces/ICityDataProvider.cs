using CodingChallenge.DataLayer.DTO;

namespace CodingChallenge.DataLayer.DataProvider.Interfaces
{
    public interface ICityDataProvider
    {
        Task<List<CityDetailsDTO>> GetZipCodeByCity(string zipCode);
    }
}
