using System.ComponentModel.DataAnnotations;

namespace HotelListing.WebAPI.DTOs.Hotal
{
    public class CreateHotelDTO:HotelBaseDTO
    {
        [Required]
        public int CountryId { get; set; }

    }
}
