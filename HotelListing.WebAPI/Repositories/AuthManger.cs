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
        private const string _loginProvider = "HotelListingAPI";
        private const string _refreshToken = "RefreshToken";


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
              expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWTSettings:DurationInMinutes"])),
                claims: claims,
                signingCredentials: credentials
                );
            // Return JwtSecurityTokenHandler
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<AuthResponseDTO> IsLoged(LoginDTO loginDTO)
        {
            var user = await _manager.FindByEmailAsync(loginDTO.Email);
            if (user is not null)
            {

                var isvalid = await _manager.CheckPasswordAsync(user, loginDTO.Password);
                if (isvalid)
                {
                    return new AuthResponseDTO()
                    {
                        UserId = user.Id,
                        Tokken = await GenerateToken(user),
                        RefreshToken=await  GenerateRefreshToken(user)
                    };
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

        public async Task<string> GenerateRefreshToken(APIUser user)
        {
            await _manager.RemoveAuthenticationTokenAsync(user, _loginProvider, _refreshToken);
            var newRefreshToken = await _manager.GenerateUserTokenAsync(user, _loginProvider, _refreshToken);
            await _manager.SetAuthenticationTokenAsync(user, _loginProvider, _refreshToken, newRefreshToken);
            return newRefreshToken;
        }
        public async Task<AuthResponseDTO> VrefiyRereshToken(AuthResponseDTO request)
        {

            #region Case You Need To read Something From Token 
            //JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            //var token = jwtSecurityTokenHandler.ReadJwtToken(request.Tokken);
            //var userName = token.Claims.AsEnumerable().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email);
            //if (userName is null)
            //{
            //    return null;
            //} 
            #endregion
            var user = await _manager.FindByIdAsync(request.UserId);
            if (user is null)
                return null;


            var isValid = await _manager.VerifyUserTokenAsync(user, _loginProvider, _refreshToken, request.Tokken);
            if (isValid)
            {
                var token = await GenerateToken(user);
                return new AuthResponseDTO()
                {
                    UserId = user.Id,
                    Tokken = token,
                    RefreshToken = await GenerateRefreshToken(user)
                };
            }
            await _manager.UpdateSecurityStampAsync(user);
            return null;
        }
    }
}
