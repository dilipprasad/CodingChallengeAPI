using CodingChallenge.Models;

namespace CodingChallenge.Business.Interfaces
{
    public interface ICityBusinessProvider
    {
        Task<List<CityDetails>> GetZipCodeByCity(string zipCode);
    }
}
