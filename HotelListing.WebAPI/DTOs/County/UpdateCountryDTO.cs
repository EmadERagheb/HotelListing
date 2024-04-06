using System.ComponentModel.DataAnnotations;

namespace HotelListing.WebAPI.DTOs.County
{
    public class UpdateCountryDTO:CountryBaseDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
