using HotelListing.WebAPI.DTOs.User;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.WebAPI.DTOs.APIUser
{
    public class UserDTO:UserBaseDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
      
    }


}
