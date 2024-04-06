using AutoMapper;
using HotelListing.Domain;
using HotelListing.WebAPI.DTOs.County;

namespace HotelListing.WebAPI.Configurations
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<Country,CreateCountryDTO>().ReverseMap();
        }
    }
}
