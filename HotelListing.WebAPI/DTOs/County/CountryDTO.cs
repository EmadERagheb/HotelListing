using HotelListing.WebAPI.DTOs.Hotal;

namespace HotelListing.WebAPI.DTOs.County
{
    public class CountryDTO:CountryBaseDTO
    {
        public int Id { get; set; }
        public List<HotelDTO> Hotels { get; set; }
    }
}

