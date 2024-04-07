using HotelListing.WebAPI.DTOs.County;

namespace HotelListing.WebAPI.DTOs.Hotal
{
    public class GetDetailHotel:HotelBaseDTO
    {
        public int Id { get; set; }

        public GetCoutryDTO Country { get; set; }
    }
}
