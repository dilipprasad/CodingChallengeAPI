using CodingChallenge.DataLayer.DTO;

namespace CodingChallenge.DataLayer.DataLayer
{
    /// <summary>
    /// This class is used for returning test data
    /// </summary>
    public class DataSource : IDataSource
    {

        /// <summary>
        /// Real world scenario wont have this
        /// </summary>
        /// <returns></returns>
        public Task<List<CityDetailsDTO>> GetCitySource()
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
