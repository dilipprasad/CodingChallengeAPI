using AutoMapper;
using CodingChallenge.DataLayer.DTO;
using CodingChallenge.Models;

namespace CodingChallenge.Business.MapperSetup
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CityDetails, CityDetailsDTO>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.ZipCode));

            //Can Add more mappings here

        }
    }
}
