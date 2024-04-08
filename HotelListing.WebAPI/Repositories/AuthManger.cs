using HotelListing.Domain;
using HotelListing.WebAPI.Contracts;
using HotelListing.WebAPI.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.WebAPI.Repositories
{
    public class AuthManger : IAuthManger
    {
        private readonly UserManager<APIUser> _manager;


        public AuthManger(UserManager<APIUser> manager)
        {
            _manager = manager;

        }

        public async Task<bool> IsLoged(LoginDTO loginDTO)
        {
            var user = await _manager.FindByEmailAsync(loginDTO.Email);
            if (user is not null)
            {
                return await _manager.CheckPasswordAsync(user, loginDTO.Password);
            }
            return false;
        }

        public async Task<IEnumerable<IdentityError>> Register(APIUser user)
        {
            var result = await _manager.CreateAsync(user, user.Password);
            if (result.Succeeded)
            {
                await _manager.AddToRoleAsync(user, "User");
            }
            return result.Errors;

        }
    }
}
