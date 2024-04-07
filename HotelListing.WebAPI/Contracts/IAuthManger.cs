using HotelListing.WebAPI.DTOs.APIUser;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.WebAPI.Contracts
{
    public interface IAuthManger
    {
        Task<IEnumerable<IdentityError>> Register(UserDTO userDTO);
    }
}
