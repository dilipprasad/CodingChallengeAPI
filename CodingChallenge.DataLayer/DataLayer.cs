using CodingChallenge.DataLayer.DTO;

namespace CodingChallenge.DataLayer
{
    /// <summary>
    /// This Implementation can be from DB or any other external API. For now  Harcoding the data
    /// </summary>
    public class DataLayer : IDataLayer
    {
        private readonly List<CityDetailsDTO> _cityDetailsSource;
        public DataLayer()
        {
            _cityDetailsSource =  GetCitySource().Result; //Awaits for result
        }

        public async Task<List<CityDetailsDTO>> GetCityDetails(string zipCode)
        {
            return await Task.FromResult<List<CityDetailsDTO>>( 

                  _cityDetailsSource.Where(x => x.ZipCode.Contains(zipCode)).ToList()
            );
        }

        /// <summary>
        /// Real world scenario wont have this
        /// </summary>
        /// <returns></returns>
        private Task<List<CityDetailsDTO>> GetCitySource()
        {
            return Task.FromResult(new List<CityDetailsDTO>
            {
                new CityDetailsDTO{City="Arizona", ZipCode="85001"  },
                new CityDetailsDTO{City="Arizona", ZipCode="85002"  },
                new CityDetailsDTO{City="Arizona", ZipCode="85003"  },
                new CityDetailsDTO{City="Arizona", ZipCode="85004"  },
                new CityDetailsDTO{City="Arizona", ZipCode="85004"  },
                new CityDetailsDTO{City="Arizona", ZipCode="85005"  },
                new CityDetailsDTO{City="Arizona", ZipCode="86556"  },

                new CityDetailsDTO{City="California", ZipCode="90001"  },
                new CityDetailsDTO{City="California", ZipCode="90002"  },
                new CityDetailsDTO{City="California", ZipCode="90003"  },
                new CityDetailsDTO{City="California", ZipCode="90004"  },
                new CityDetailsDTO{City="California", ZipCode="90005"  },
                new CityDetailsDTO{City="California", ZipCode="90006"  },
                new CityDetailsDTO{City="California", ZipCode="96162"  },

            });
        }
    }
}
