using CodingChallenge.DataLayer.DataLayer;
using CodingChallenge.DataLayer.DTO;

namespace CodingChallenge.DataLayer.DataAdaptor
{
    /// <summary>
    /// This Implementation can be from DB or any other external API. For now  Harcoding the data
    /// </summary>
    public class DataLayer : IDataLayer
    {
        private readonly List<CityDetailsDTO> _cityDetailsSource;
        public DataLayer(IDataSource dataSrc)
        {
            _cityDetailsSource = dataSrc.GetCitySource().Result; //Awaits for result
        }

        public async Task<List<CityDetailsDTO>> SearchCityDetailsByZipCode(string zipCode)
        {
            return await Task.FromResult(

                  _cityDetailsSource.Where(x => x.ZipCode.Contains(zipCode)).ToList()
            );
        }

        //May come handy in future
        public async Task<List<CityDetailsDTO>> SearchCityDetailsByName(string cityName)
        {
            return await Task.FromResult(

                  _cityDetailsSource.Where(x => x.City.Contains(cityName)).ToList()
            );
        }

    }
}
