using AutoMapper;
using HotelListing.Domain;
using HotelListing.WebAPI.Contracts;
using HotelListing.WebAPI.DTOs.APIUser;
using HotelListing.WebAPI.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthManger _authManger;
        private readonly IMapper _mapper;

        public AccountsController(IAuthManger authManger, IMapper mapper)
        {
            _authManger = authManger;
            _mapper = mapper;
        }


        [HttpPost("/register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/Accounts/register
        public async Task<ActionResult> Resiter(UserDTO userDTO)
        {
            var user = _mapper.Map<APIUser>(userDTO);
            user.UserName = userDTO.Email;
            IEnumerable<IdentityError>? errors = await _authManger.Register(user);
            if (!errors.Any())
            {
                return Ok("Register Success");
            }
            else
            {
                foreach (var error in errors)
                {

                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("/Login")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login (LoginDTO loginDTO)
        {
          var isvalid =   await  _authManger.IsLoged(loginDTO);
            return isvalid ? Ok() : Unauthorized();
        }
    }
}
