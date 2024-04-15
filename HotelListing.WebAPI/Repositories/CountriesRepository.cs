using AutoMapper;
using HotelListing.Data;
using HotelListing.Domain;
using HotelListing.WebAPI.Contracts;

namespace HotelListing.WebAPI.Repositories
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        public CountriesRepository(HotelListingDbcontext context,IMapper mapper ) : base(context,mapper)
        {
        }
    }
}
