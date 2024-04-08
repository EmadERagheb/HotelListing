using HotelListing.Domain;
using HotelListing.WebAPI.Contracts;
using HotelListing.WebAPI.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.WebAPI.Repositories
{

    public class AuthManger : IAuthManger
    {
        private readonly UserManager<APIUser> _manager;
        private readonly IConfiguration _configuration;

        public AuthManger(UserManager<APIUser> manager, IConfiguration configuration)
        {
            _manager = manager;
            _configuration = configuration;
        }

        public async Task<string> GenerateToken(APIUser user)
        {
            // Token Security
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Claims
            //Get Roles Claims
            var roles = await _manager.GetRolesAsync(user);
            List<Claim> roleClims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();
            //Get User Claims From DB If Exist
            var userClaims = await _manager.GetClaimsAsync(user);

            //Union Claims And Add Additional Claims
            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Sub,user.FirstName+" "+user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),

            }.Union(roleClims).Union(userClaims);
            //build Token
            var token = new JwtSecurityToken(
                issuer: _configuration["JWTSettings:Issuer"],
                audience: _configuration["JWTSettings:Audience"],
              expires  : DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWTSettings:DurationInMinutes"])),
                claims: claims,
                signingCredentials: credentials
                );
            // Return JwtSecurityTokenHandler
            return new JwtSecurityTokenHandler().WriteToken(token); 

        }

        public async Task<AuthRespondDTO> IsLoged(LoginDTO loginDTO)
        {
            var user = await _manager.FindByEmailAsync(loginDTO.Email);
            if (user is not null)
            {
               
                  var isvalid = await _manager.CheckPasswordAsync(user, loginDTO.Password);
                if (isvalid)
                {
                    return new AuthRespondDTO() { UserId = user.Id, Tokken = await GenerateToken(user) };
                }
            }
            return null;
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
