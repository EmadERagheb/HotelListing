using System.ComponentModel.DataAnnotations;

namespace HotelListing.WebAPI.DTOs.County
{
    public abstract class CountryBaseDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }
    }
}
