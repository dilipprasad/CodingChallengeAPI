using CodingChallenge.Models;

namespace CodingChallenge.Business.Interfaces
{
    public interface ICityDataProvider
    {
        Task<CityDetails> GetZipCodeByCity(string zipCode);
    }
}
