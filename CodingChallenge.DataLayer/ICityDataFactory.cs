using CodingChallenge.Models;

namespace CodingChallenge.DataLayer
{
    public interface ICityDataFactory
    {
        Task<CityDetails> GetZipCodeByCity(string zipCode);
    }
}
