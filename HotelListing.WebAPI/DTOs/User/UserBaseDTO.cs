using System.ComponentModel.DataAnnotations;

namespace HotelListing.WebAPI.DTOs.User
{
    public abstract class UserBaseDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
