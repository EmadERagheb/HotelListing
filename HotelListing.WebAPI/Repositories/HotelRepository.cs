using AutoMapper;
using HotelListing.Data;
using HotelListing.Domain;
using HotelListing.WebAPI.Contracts;

namespace HotelListing.WebAPI.Repositories
{
    public class HotelRepository : GenericRepository<Hotel>,IHotelRepository
    {
        public HotelRepository(HotelListingDbcontext context,IMapper mapper) : base(context, mapper)
        {
        }
    }
}
