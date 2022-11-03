using CodingChallenge.DataLayer.DTO;

namespace CodingChallenge.DataLayer
{
    public interface ICityDataFactory
    {
        Task<CityDetailsDTO> GetZipCodeByCity(string zipCode);
    }
}
