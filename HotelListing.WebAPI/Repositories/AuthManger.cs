using AutoMapper;
using HotelListing.Domain;
using HotelListing.WebAPI.Contracts;
using HotelListing.WebAPI.DTOs.APIUser;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.WebAPI.Repositories
{
    public class AuthManger : IAuthManger
    {
        private readonly UserManager<APIUser> _manager;
        private readonly IMapper _mapper;

        public AuthManger(UserManager<APIUser> manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }
        public async Task<IEnumerable<IdentityError>> Register(UserDTO userDTO)
        {
            var user = _mapper.Map<APIUser>(userDTO);
            user.UserName = user.Email;
            var result = await _manager.CreateAsync(user, userDTO.Password);

            if(result.Succeeded)
            {
              await  _manager.AddToRoleAsync(user, "User");
            }
            return result.Errors;

        }
    }
}
