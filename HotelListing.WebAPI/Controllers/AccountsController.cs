using Asp.Versioning;
using AutoMapper;
using HotelListing.Domain;
using HotelListing.WebAPI.Contracts;
using HotelListing.WebAPI.DTOs.APIUser;
using HotelListing.WebAPI.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace HotelListing.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthManger _authManger;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IAuthManger authManger, IMapper mapper, ILogger<AccountsController> logger)
        {
            _authManger = authManger;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpPost("/register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/Accounts/register
        public async Task<ActionResult> Resiter(UserDTO userDTO)
        {
            _logger.LogInformation($"registration attempt for user{userDTO.Email}");
            try
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
            catch (Exception e)
            {
                _logger.LogError($"something went wrong is the {nameof(Register)} for user of email:{userDTO.Email}");
                return Problem($"something went wrong in {nameof(Register)}", statusCode: 500);
            }

        }

        [HttpPost]
        [Route("/Login")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDTO)
        {
            try
            {
                _logger.LogInformation($"login attempt from email:{loginDTO.Email}");
                var respondDTO = await _authManger.IsLoged(loginDTO);
                return respondDTO is not null ? Ok(respondDTO) : Unauthorized();
            }
            catch (Exception)
            {
                _logger.LogError($"something went wrong during {nameof(Login)}-the attempt of user {loginDTO.Email}");
                return Problem("something went wrong", statusCode: 500);
                
            }
       

        }
        [HttpPost]
        [Route("/RefreshToken")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthResponseDTO>> RefreshToken(AuthResponseDTO request)
        {
            var result = await _authManger.VrefiyRereshToken(request);
            if (result is null)
            {
                return Unauthorized();
            }
            else
                return Ok(result);

        }
    }
}
