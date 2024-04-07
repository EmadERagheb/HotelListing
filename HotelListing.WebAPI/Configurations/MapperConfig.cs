using AutoMapper;
using HotelListing.Domain;
using HotelListing.WebAPI.DTOs.County;
using HotelListing.WebAPI.DTOs.Hotal;

namespace HotelListing.WebAPI.Configurations
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<Country,CreateCountryDTO>().ReverseMap();
            CreateMap<Country,GetCoutryDTO>().ReverseMap();
            CreateMap<Country,CountryDTO>().ReverseMap();
            CreateMap<Country,UpdateCountryDTO>().ReverseMap() ;

            CreateMap<Hotel,HotelDTO>().ReverseMap();
            CreateMap<Hotel,GetHotelDTO>().ReverseMap() ;
            CreateMap<Hotel,GetDetailHotel>().ReverseMap() ;
            CreateMap<Hotel, CreateHotelDTO>().ReverseMap();
        }
    }
}
