using AutoMapper;
using CodingChallenge.DataLayer.DTO;
using CodingChallenge.Models;

namespace CodingChallenge.Business.MapperSetup
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CityDetails, CityDetailsDTO>();
        }
    }
}
