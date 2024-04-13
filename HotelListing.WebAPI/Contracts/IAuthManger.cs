using HotelListing.Domain;
using HotelListing.WebAPI.DTOs.APIUser;
using HotelListing.WebAPI.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.WebAPI.Contracts
{
    public interface IAuthManger
    {
        Task<IEnumerable<IdentityError>> Register(APIUser user);

        Task<AuthResponseDTO> IsLoged(LoginDTO loginDTO);

        Task<string> GenerateToken(APIUser user);

        //Very Important
        /// <summary>
        /// To Work With Refresh token you need 
        /// add token provider which we set at configuration  to identity configuration at program.cs
        /// and Add.DefaultToken Profiver
     
        Task<string> GenerateRefreshToken(APIUser user);
        Task<AuthResponseDTO> VrefiyRereshToken(AuthResponseDTO request);

    }
}
