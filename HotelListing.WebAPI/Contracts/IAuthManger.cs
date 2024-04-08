using HotelListing.Domain;
using HotelListing.WebAPI.DTOs.APIUser;
using HotelListing.WebAPI.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.WebAPI.Contracts
{
    public interface IAuthManger
    {
        Task<IEnumerable<IdentityError>> Register(APIUser user);

        Task<AuthRespondDTO> IsLoged(LoginDTO loginDTO);

        Task<string> GenerateToken(APIUser user);
    }
}
