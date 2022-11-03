using CodingChallenge.Models;

namespace CodingChallenge.Business.Interfaces
{
    public interface ICityBusinessProvider
    {
        Task<CityDetails> GetZipCodeByCity(string zipCode);
    }
}
