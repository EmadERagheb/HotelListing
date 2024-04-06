using System.ComponentModel.DataAnnotations;

namespace HotelListing.WebAPI.DTOs.County
{
    public class CreateCountryDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ShortName { get; set; }

    }
}
